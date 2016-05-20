// This file is part of the MS.Gamification project
// 
// File: ObservationControllerSpecs.cs  Created: 2016-05-10@22:29
// Last modified: 2016-05-20@22:55

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FakeItEasy;
using Machine.Specifications;
using Machine.Specifications.Annotations;
using MS.Gamification.Controllers;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using MS.Gamification.Tests.TestHelpers.Fakes;

/*
Observation controller behaviours

    Submit GET action
    =================

    Given a logged in user,
    + When the Submit action is called with a non-existent Challenge ID, it should return HTTP 404 Not Found

    Given a valid logged in user and a ChallengeID that exists but has not been unlocked by the user
    - When the Submit action is called, it should return HTTP Invalid Request

    Given a valid request (existing Challenge ID) and a logged in user,
    - When the Submit action is called, It should return the Submit view.

    Submit POST action
    ==================

    Given a valid submission and valid logged in user
    - When the Submit POST action is called
        + It should convert the local date/time to UTC date/time
        + It should save the submission in the database
        + It should add the submission to the moderation queue in Pending state

*/

namespace MS.Gamification.Tests.Controllers
    {

    #region context base classes
    public class with_observation_controller_and_fake_repository
        {
        protected static IRepository<Observation, int> observationRepository;

        protected static ObservationController controller;
        protected static List<Observation> fakeData;
        protected static IUnitOfWork uow;

        [UsedImplicitly] Cleanup after = () =>
            {
            fakeData = null;
            uow = null;
            observationRepository = null;
            controller = null;
            };

        [UsedImplicitly] Establish context = () =>
            {
            fakeData = new List<Observation>();
            uow = A.Fake<IUnitOfWork>();
            observationRepository = A.Fake<IRepository<Observation, int>>();
            A.CallTo(() => observationRepository.GetAll()).Returns(fakeData);
            A.CallTo(() => uow.ObservationsRepository).Returns(observationRepository);
            controller = new ObservationController(uow);
            };
        }
    #endregion context base classes

    [Subject(typeof(ObservationController), "non-existing challenge")]
    public class when_submit_is_called_with_a_non_existing_challenge : with_observation_controller_and_fake_repository
        {
        Establish context = () =>
            {
            var fakeChallengesRepository = A.Fake<IRepository<Challenge, int>>();
            A.CallTo(() => fakeChallengesRepository.GetMaybe(A<int>.Ignored)).Returns(Maybe<Challenge>.Empty);
            A.CallTo(() => uow.ChallengesRepository).Returns(fakeChallengesRepository);
            };

        Because of = () => actionResult = controller.SubmitObservation(10);
        It should_return_404_not_found = () => actionResult.ShouldBeOfExactType<HttpNotFoundResult>();
        static ActionResult actionResult;
        }

    [Subject(typeof(ObservationController), "valid request")]
    public class when_submit_is_called_with_a_valid_challenge : with_observation_controller_and_fake_repository
        {
        Establish context = () =>
            {
            var fakeChallengesRepository = A.Fake<IRepository<Challenge, int>>();
            var validChallenge = new Challenge
                {
                Id = ValidChallengeId,
                Name = "Unit test challenge",
                Category = new Category {Id = 1, Name = "Unit Test"},
                Location = "Your Imagination",
                Points = 10
                };
            var maybechallenge = new Maybe<Challenge>(validChallenge);
            A.CallTo(() => fakeChallengesRepository.GetMaybe(ValidChallengeId)).Returns(maybechallenge);
            A.CallTo(() => uow.ChallengesRepository).Returns(fakeChallengesRepository);
            };

        Because of = () => Result = controller.SubmitObservation(ValidChallengeId) as ViewResult;
        It should_return_the_default_view_by_convention = () => Result.ViewName.ShouldBeEmpty();
        static ViewResult Result;
        const int ValidChallengeId = 1;
        }

    [Subject(typeof(ObservationController), "valid POST request")]
    public class when_a_valid_submission_is_posted : with_observation_controller_and_fake_repository
        {
        Establish context = () =>
            {
            observationDateTimeLocal = new DateTime(2000, 6, 1, 00, 00, 00);
            observationDateTimeUtc = observationDateTimeLocal.AddHours(-1);
            model = new SubmitObservationViewModel
                {
                Challenge = new Challenge
                    {
                    Id = 1,
                    Name = "Unit test challenge",
                    Category = new Category {Id = 1, Name = "Unit Test"},
                    Location = "Your Imagination",
                    Points = 10
                    },
                Transparency = TransparencyLevel.Clear,
                Seeing = AntoniadiScale.MostlyStable,
                ObservationDateTimeLocal = observationDateTimeLocal,
                Equipment = ObservingEquipment.NakedEye,
                ObservingSite = "Unit Test",
                Notes = "Unit Test",
                SubmittedImage = "CorrectImage.png",
                ValidationImages =
                new List<string> {"WrongImage1.png", "WrongImage2.png", "CorrectImage.png", "WrongImage4.png"}
                };
            var identity = new FakeIdentity("Valid User");
            var user = new FakePrincipal(identity, new string[] {});
            var httpContext = new FakeHttpContext("/Observation/SubmitObservation");
            httpContext.User = user;
            var controllerContext = new ControllerContext();
            controllerContext.HttpContext = httpContext;
            controller.ControllerContext = controllerContext;
            controller.TempData[nameof(Challenge)] = model.Challenge;
            };

        Because of = () => Result = controller.SubmitObservation(model) as ViewResult;
        It should_save_the_observation_time_in_universal_time =
            () =>
                A.CallTo(
                    () =>
                        observationRepository.Add(
                            A<Observation>.That.Matches(o => o.ObservationDateTimeUtc == observationDateTimeUtc)))
                    .MustHaveHappened(Repeated.Exactly.Once);
        It should_save_the_submission_in_the_database =
            () =>
                {
                A.CallTo(() => observationRepository.Add(A<Observation>.Ignored)).MustHaveHappened();
                A.CallTo(() => uow.Commit()).MustHaveHappened(Repeated.Exactly.Once);
                };

        It should_add_the_submission_to_the_moderation_queue = () => A.CallTo(
            () =>
                observationRepository.Add(
                    A<Observation>.That.Matches(o => o.Status == ModerationState.AwaitingModeration)))
            .MustHaveHappened(Repeated.Exactly.Once);


        //- It should add the submission to the moderation queue
        //- It should set the moderation state to Pending

        static ViewResult Result;
        static SubmitObservationViewModel model;
        static DateTime observationDateTimeLocal;
        static DateTime observationDateTimeUtc;
        }
    }