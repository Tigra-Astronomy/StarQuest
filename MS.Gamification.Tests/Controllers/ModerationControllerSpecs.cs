// This file is part of the MS.Gamification project
// 
// File: ModerationControllerSpecs.cs  Created: 2016-05-20@23:03
// Last modified: 2016-05-22@20:07

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.Models;

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
    class when_the_index_action_is_called : with_mvc_controller<ModerationController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithModerator("mod", "Joe Moderator")
            .WithStandardUser("user", "Joe User")
            .WithEntity(new Category {Id = 1, Name = "Category"})
            .WithEntity(new Challenge {Id = 1, Name = "Unit Test Challenge", CategoryId = 1})
            .WithEntity(new Observation {Id = 1, Status = ModerationState.Approved, UserId = "user", ChallengeId = 1})
            .WithEntity(new Observation {Id = 2, Status = ModerationState.AwaitingModeration, UserId = "user", ChallengeId = 1})
            .WithEntity(new Observation {Id = 3, Status = ModerationState.Rejected, UserId = "user", ChallengeId = 1})
            .WithRoute("/Moderation/Index", HttpVerbs.Get)
            .WithRequestingUser("Joe Moderator", "Moderator")
            .Build();
        Because of = () => { Result = ControllerUnderTest.Index() as ViewResult; };
        It should_return_the_index_view = () => Result.ViewName.ShouldEqual(string.Empty);
        It should_populate_the_viewmodel_with_the_observations_awaiting_moderation =
            () => ((IEnumerable<ModerationQueueItem>) Result.Model).Single().ObservationId.ShouldEqual(2);
        static ViewResult Result;
        }
    }