// This file is part of the MS.Gamification project
// 
// File: ModerationControllerSpecs.cs  Created: 2016-11-01@19:37
// Last modified: 2016-12-12@23:25

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using MS.Gamification.Tests.TestHelpers;
using MS.Gamification.ViewModels.Moderation;

namespace MS.Gamification.Tests.Controllers
    {
    /*
     * ModerationController specs
     * 
     * Given: a logged in moderator, some observations with status Submitted
     * When: a call is made to the index action
     * Then: 
     *  it should return the index view
     *  it should populate the ViewModel with the observations  with Status AwaitingModeration
     */

    [Subject(typeof(ModerationController), "Index")]
    class when_the_index_action_is_called : with_standard_mission<ModerationController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithRoute("/Moderation/Index", HttpVerbs.Get)
            .WithRequestingUser("Joe Moderator", "Moderator")
            .WithData(d => d
                    .WithModerator("mod", "Joe Moderator")
                    .WithStandardUser("user", "Joe User")
                    .WithEntity(new Observation {Id = 1, Status = ModerationState.Approved, UserId = "user", ChallengeId = 100})
                    .WithEntity(new Observation
                            {Id = 2, Status = ModerationState.AwaitingModeration, UserId = "user", ChallengeId = 100})
                    .WithEntity(new Observation {Id = 3, Status = ModerationState.Rejected, UserId = "user", ChallengeId = 100})
            )
            .Build();
        Because of = () => { Result = ControllerUnderTest.Index() as ViewResult; };
        It should_return_the_index_view = () => Result.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewmodel_with_the_observations_awaiting_moderation =
            () => ((IEnumerable<ModerationQueueItem>) Result.Model).Single().ObservationId.ShouldEqual(2);
        static ViewResult Result;
        }

    [Subject(typeof(ModerationController), "Details")]
    class when_getting_and_the_id_parameter_is_null : with_mvc_controller<ModerationController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Result = ControllerUnderTest.Details(null) as HttpStatusCodeResult;
        It should_return_400_bad_request = () => Result.StatusCode.ShouldEqual(400);
        static HttpStatusCodeResult Result;
        }

    [Subject(typeof(ModerationController), "Details")]
    class when_getting_an_id_that_is_not_in_the_database : with_standard_mission<ModerationController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithData(d => d
                    .WithStandardUser("user", "Joe User")
                    .WithEntity(
                        new Observation
                            {
                            Id = 2, Status = ModerationState.AwaitingModeration, UserId = "user", ChallengeId = 100
                            })
            )
            .Build();
        Because of = () => Result = ControllerUnderTest.Details(9); // Only ID=2 exists
        It should_return_404_not_found = () => Result.ShouldBeOfExactType<HttpNotFoundResult>();
        static ActionResult Result;
        }

    [Subject(typeof(ModerationController), "Details")]
    class when_getting_a_valid_observation_id : with_standard_mission<ModerationController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithData(d => d
                .WithStandardUser("user", "Joe User")
                .WithEntity(new Observation
                        {Id = 2, Status = ModerationState.AwaitingModeration, UserId = "user", ChallengeId = 100}))
            .Build();
        Because of = () => Result = (ViewResult) ControllerUnderTest.Details(2);
        It should_return_the_default_view = () => Result.ViewName.ShouldBeEmpty();
        It should_populate_the_model_with_the_specified_entity = () => ((Observation) Result.Model).Id.ShouldEqual(2);
        static ViewResult Result;
        }

    [Subject(typeof(ModerationController), "Approve")]
    class when_posting_an_approval_for_an_invalid_id : with_mvc_controller<ModerationController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithData(d => d
                .WithStandardUser("user", "Joe User")
                .WithEntity(new Category {Id = 1, Name = "Category"})
                .WithEntity(new Badge {Id = 1, Name = "Badge 1"})
                .WithEntity(new Mission {Id = 1, Title = "Unit Test Mission"})
                .WithEntity(new MissionLevel {Id = 1, MissionId = 1})
                .WithEntity(new MissionTrack {Id = 1, Number = 1, Name = "Track 1", BadgeId = 1, MissionLevelId = 1})
                .WithEntity(new Challenge {Id = 1, Name = "Unit Test Challenge", CategoryId = 1, MissionTrackId = 1})
                .WithEntity(new Observation
                        {Id = 2, Status = ModerationState.AwaitingModeration, UserId = "user", ChallengeId = 1}))
            .Build();
        Because of = () => Result = ControllerUnderTest.Approve(99).WaitForResult();
        It should_return_404_not_found = () => Result.ShouldBeOfExactType<HttpNotFoundResult>();
        static ActionResult Result;
        }

    [Subject(typeof(ModerationController), "Reject")]
    class when_posting_a_rejection_for_an_invalid_id : with_mvc_controller<ModerationController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithData(d => d
                .WithStandardUser("user", "Joe User")
                .WithEntity(new Category {Id = 1, Name = "Category"})
                .WithEntity(new Challenge {Id = 1, Name = "Unit Test Challenge", CategoryId = 1})
                .WithEntity(new Observation
                        {Id = 2, Status = ModerationState.AwaitingModeration, UserId = "user", ChallengeId = 1}))
            .Build();
        Because of = () => Result = ControllerUnderTest.Reject(99);
        It should_return_404_not_found = () => Result.ShouldBeOfExactType<HttpNotFoundResult>();
        static ActionResult Result;
        }

    [Subject(typeof(ModerationController), "Approve")]
    class when_posting_an_approval_for_a_valid_observation : with_standard_mission<ModerationController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.WithData(d => d
                .WithStandardUser("user", "Joe User")
                .WithObservation().ForChallenge(100).WithId(2).ForUserId("user").AwaitingModeration().BuildObservation())
            .Build();
        Because of = () => Result = (RedirectToRouteResult) ControllerUnderTest.Approve(2).WaitForResult();
        It should_change_the_observation_status_to_approved =
            () => UnitOfWork.Observations.Get(2).Status.ShouldEqual(ModerationState.Approved);
        It should_redirect_to_the_index_action = () => Result.RouteValues["Action"].ShouldEqual("Index");
        static RedirectToRouteResult Result;
        }

    [Subject(typeof(ModerationController), "Reject")]
    class when_posting_an_rejection_for_a_valid_observation : with_standard_mission<ModerationController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.WithData(d => d
                .WithStandardUser("user", "Joe User")
                .WithObservation().ForChallenge(100).WithId(2).ForUserId("user").AwaitingModeration().BuildObservation())
            .Build();
        Because of = () => Result = (RedirectToRouteResult) ControllerUnderTest.Reject(2);

        It should_change_the_observation_status_to_rejected =
            () => UnitOfWork.Observations.Get(2).Status.ShouldEqual(ModerationState.Rejected);
        It should_redirect_to_the_index_action = () => Result.RouteValues["Action"].ShouldEqual("Index");
        static RedirectToRouteResult Result;
        }
    }