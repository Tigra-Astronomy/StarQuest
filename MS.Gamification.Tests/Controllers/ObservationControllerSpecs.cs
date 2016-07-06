// This file is part of the MS.Gamification project
// 
// File: ObservationControllerSpecs.cs  Created: 2016-05-10@22:29
// Last modified: 2016-07-04@22:29

using System;
using System.Linq;
using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

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
    [Subject(typeof(ObservationController), "non-existing challenge")]
    class when_an_observation_with_a_non_existent_challenge_is_submitted : with_mvc_controller<ObservationController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Result = ControllerUnderTest.SubmitObservation(10);
        It should_return_404_not_found = () => Result.ShouldBeOfExactType<HttpNotFoundResult>();
        static ActionResult Result;
        }


    [Subject(typeof(ObservationController), "valid request")]
    class when_submit_is_called_with_a_valid_challenge : with_standard_mission<ObservationController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Result = ControllerUnderTest.SubmitObservation(100) as ViewResult;
        It should_return_the_default_view_by_convention = () => Result.ViewName.ShouldBeEmpty();
        It should_include_the_challenge_in_the_view_model =
            () => ((SubmitObservationViewModel) Result.Model).Challenge.Id.ShouldEqual(100);
        It should_put_the_challenge_into_tempdata =
            () => ((Challenge) Result.TempData[nameof(Challenge)]).Id.ShouldEqual(100);
        static ViewResult Result;
        }

    [Subject(typeof(ObservationController), "valid POST request")]
    [Ignore("The controller needs to be re-written so that it doesn't use TempData")]
    class when_a_valid_submission_is_posted : with_standard_mission<ObservationController>
        {
        Establish context = () =>
            {
            ObservationDateTimeLocal = new DateTime(2000, 6, 1, 00, 00, 00);
            ObservationDateTimeUtc = ObservationDateTimeLocal.AddHours(-1);

            model = new SubmitObservationViewModel
                {
                Transparency = TransparencyLevel.Clear,
                Seeing = AntoniadiScale.MostlyStable,
                ObservationDateTimeLocal = ObservationDateTimeLocal,
                Equipment = ObservingEquipment.NakedEye,
                ObservingSite = "Unit Test",
                Notes = "Unit Test",
                SubmittedImage = "CorrectImage.png"
                };
            ControllerUnderTest = ContextBuilder
                .WithEntity(new Category {Id = 1, Name = "Unit Test Category"})
                .WithEntity(Challenge = new Challenge
                    {
                    Id = 1,
                    Name = "Unit test challenge",
                    CategoryId = 1,
                    Location = "Your Imagination",
                    Points = 10
                    })
                .WithTempData(nameof(Challenge), Challenge)
                .Build();
            };

        Because of = () => Result = ControllerUnderTest.SubmitObservation(model) as ViewResult;
        It should_save_the_observation_time_in_universal_time =
            () => ContextBuilder.UnitOfWork.Observations
                .GetAll()
                .Single()
                .ObservationDateTimeUtc
                .ShouldEqual(ObservationDateTimeUtc);
        It should_save_the_submission_in_the_database =
            () => ContextBuilder.UnitOfWork.Observations.GetAll().Count().ShouldEqual(1);

        It should_add_the_submission_to_the_moderation_queue = () =>
            ContextBuilder.UnitOfWork.Observations.GetAll()
                .Single()
                .Status.ShouldEqual(ModerationState.AwaitingModeration);
        static ViewResult Result;
        static SubmitObservationViewModel model;
        static DateTime ObservationDateTimeLocal;
        static DateTime ObservationDateTimeUtc;
        static Challenge Challenge;
        }
    }