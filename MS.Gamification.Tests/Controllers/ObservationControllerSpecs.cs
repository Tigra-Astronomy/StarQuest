// This file is part of the MS.Gamification project
// 
// File: ObservationControllerSpecs.cs  Created: 2016-04-22@21:49
// Last modified: 2016-04-24@18:15 by Fern

using System.Collections.Generic;
using System.Web.Mvc;
using FakeItEasy;
using Machine.Specifications;
using Machine.Specifications.Annotations;
using MS.Gamification.Controllers;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;

/*
Observation controller behaviours

    Given a logged in user,
    + When the Submit action is called with a non-existent Challenge ID, it should return HTTP 404 Not Found

    Given a valid logged in user and a ChallengeID that exists but has not been unlocked by the user
    - When the Submit action is called, it should return HTTP Invalid Request

    Given a valid request (existing Challenge ID) and a logged in user,
    - When the Submit action is called, It should return the Submit view.

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

        [UsedImplicitly]
        Cleanup after = () =>
            {
            fakeData = null;
            uow = null;
            observationRepository = null;
            controller = null;
            };

        [UsedImplicitly]
        Establish context = () =>
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

        Because of = () => actionResult = controller.SubmitObservation(id: 10);
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

        Because of = () => Result = controller.SubmitObservation(id: ValidChallengeId) as ViewResult;
        It should_return_the_default_view_by_convention = () => Result.ViewName.ShouldBeEmpty();
        static ViewResult Result;
        const int ValidChallengeId = 1;
        }
    }
