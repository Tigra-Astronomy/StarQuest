// This file is part of the MS.Gamification project
// 
// File: BulkCreateObservationsSpecs.cs  Created: 2016-07-24@06:21
// Last modified: 2016-07-24@12:38

using System;
using System.Linq;
using Machine.Specifications;
using MS.Gamification.GameLogic;
using MS.Gamification.Models;
using MS.Gamification.Tests.Controllers;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.GameLogic
    {
    /*
     * Rules for bulk-creating observations
     * 
     * + A bulk observation can only be created if the user has no other observation for the same challenge on the same day.
     * + The validation image should automatically be correct.
     * ? Bulk-creating observations for locked challenges is allowed (no specific test)
     * + The status should be "Accepted".
     * + The operation should return a summary of what happened, e.g. list of error messages, count of successful and failed operations.
     */

    [Subject(typeof(GameRulesService), "bulk observations")]
    class when_creating_an_observation : with_mvc_controller<DummyGameController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .WithStandardUser("user", "Joe User")
                .WithEntity(new Category {Id = 1, Name = "Test"})
                .WithMissionLevel(1)
                .WithTrack(1)
                .WithChallenge("Unit test challenge").WithId(99).InCategory(1)
                .WithValidationImage("unit-test-image").BuildChallenge()
                .BuildTrack()
                .BuildMission()
                .Build();
            observation = new Observation {ChallengeId = 99};
            GameService = ContextBuilder.RulesService;
            };
        Because of = () => result = GameService.BatchCreateObservations(observation, new[] {"user"});
        It should_contain_the_correct_validation_image_for_the_challenge = () =>
            UnitOfWork.Observations.GetAll().Single().SubmittedImage.ShouldEqual("unit-test-image");
        It should_be_approved = () =>
            UnitOfWork.Observations.GetAll().Single().Status.ShouldEqual(ModerationState.Approved);

        It should_report_one_success = () => result.Succeeded.ShouldEqual(1);
        It should_report_no_errors = () => result.Failed.ShouldEqual(0);
        static IGameEngineService GameService;
        static Observation observation;
        static BatchCreateObservationsResult result;
        }

    [Subject(typeof(GameRulesService), "bulk observations")]
    class when_creating_an_observation_for_a_user_who_already_has_that_observation_on_that_day
        : with_mvc_controller<DummyGameController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .WithStandardUser("user", "Joe User")
                .WithEntity(new Category {Id = 1, Name = "Test"})
                .WithMissionLevel(1)
                .WithTrack(1)
                .WithChallenge("Unit test challenge").WithId(99).InCategory(1)
                .WithValidationImage("unit-test-image").BuildChallenge()
                .BuildTrack()
                .BuildMission()
                .WithObservation().ForChallenge(99).ForUserId("user").At(StartOfDay).BuildObservation()
                .Build();
            observation = new Observation {ChallengeId = 99, ObservationDateTimeUtc = EndOfDay};
            GameService = ContextBuilder.RulesService;
            };
        Because of = () => result = GameService.BatchCreateObservations(observation, new[] {"user"});
        It should_report_no_successes = () => result.Succeeded.ShouldEqual(0);
        It should_report_one_errors = () => result.Failed.ShouldEqual(1);
        It should_emit_one_error_message = () => result.Errors.Keys.Count.ShouldEqual(1);
        It should_reference_the_username_in_the_error = () => result.Errors.Keys.Single().ShouldEqual("Joe User");
        static IGameEngineService GameService;
        static Observation observation;
        static BatchCreateObservationsResult result;
        static readonly DateTime StartOfDay = DateTime.SpecifyKind(new DateTime(2016, 7, 24, 0, 0, 0), DateTimeKind.Utc);
        static readonly DateTime EndOfDay = StartOfDay + TimeSpan.FromDays(1) - TimeSpan.FromTicks(1);
        }
    }