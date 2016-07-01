// This file is part of the MS.Gamification project
// 
// File: MissionControllerSpecs.cs  Created: 2016-07-01@07:36
// Last modified: 2016-07-01@20:42

using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

/*
 * Mission Controller Behaviours
 * 
 * When no levelId parameter is supplied (null)
 *      + The returned View should be based on Level 1
 * When the mission ID is invalid
 *      - Should return a 404 Not Found error
 */

namespace MS.Gamification.Tests.Controllers
    {
    [Subject(typeof(MissionController), "Level ID omitted")]
    internal class when_invoking_the_level_action_with_no_id_parameter : with_mvc_controller<MissionController>
        {
        private Establish context = () => ControllerUnderTest = ContextBuilder
            .WithEntity(new Mission
                {
                AwardTitle = "unit test",
                Id = 1,
                Level = 1,
                Name = "unit test mission 1"
                })
            .Build();
        private Because of = () => result = (ViewResult) ControllerUnderTest.Level(1, null);
        private It should_assume_level_1 = () => ((MissionProgressViewModel) result.Model).Level.ShouldEqual(1);
        private static ViewResult result;
        }

    [Subject(typeof(MissionController), "Invalid Mission ID")]
    internal class when_an_invalid_mission_id_is_specified : with_mvc_controller<MissionController>
        {
        private Establish context = () => ControllerUnderTest = ContextBuilder
            .WithEntity(new Mission
                {
                AwardTitle = "unit test",
                Id = 1,
                Level = 1,
                Name = "unit test mission 1"
                })
            .Build();
        private Because of = () => result = (HttpStatusCodeResult) ControllerUnderTest.Level(InvalidMissionId, null);
        private It should_return_404 = () => result.StatusCode.ShouldEqual(404);
        private static HttpStatusCodeResult result;
        private const int InvalidMissionId = 99;
        }
    }