// This file is part of the MS.Gamification project
// 
// File: MissionControllerSpecs.cs  Created: 2016-07-01@07:36
// Last modified: 2016-07-02@03:49

using System.Linq;
using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.Tests.TestHelpers;
using MS.Gamification.ViewModels;

/*
 * Mission Controller Behaviours
 * 
 * Given: A single mission with ID=1 and a single level with number 1.
 * When no levelId parameter is supplied (null)
 *      + The returned View should be based on Level 1
 * When the mission ID is invalid
 *      + Should return a 404 Not Found error
 * When the mission ID is valid but the Level number is > 1
 *      - Should return a 404 Not Found
 * 
 * Given: A mission with the following details:
 *          Mission ID=1
 *          Level = 1
 *          Track 1:
 *              Name
 */

namespace MS.Gamification.Tests.Controllers
    {
    [Subject(typeof(MissionController), "Level ID omitted")]
    internal class when_invoking_the_level_action_with_no_id_parameter : with_mvc_controller<MissionController>
        {
        private Establish context = () => ControllerUnderTest = ContextBuilder
            .WithMission().WithId(1).BuildMission()
            .Build();
        private Because of = () => result = (ViewResult) ControllerUnderTest.Level(1, null);
        private It should_assume_level_1 = () => ((MissionProgressViewModel) result.Model).Level.ShouldEqual(1);
        private static ViewResult result;
        }

    [Subject(typeof(MissionController), "Invalid Mission ID")]
    internal class when_an_invalid_mission_id_is_specified : with_mvc_controller<MissionController>
        {
        private Establish context = () => ControllerUnderTest = ContextBuilder
            .WithMission().BuildMission()
            .Build();
        private Because of = () => result = (HttpStatusCodeResult) ControllerUnderTest.Level(InvalidMissionId, null);
        private It should_return_404 = () => result.StatusCode.ShouldEqual(404);
        private static HttpStatusCodeResult result;
        private const int InvalidMissionId = 99;
        }

    [Subject(typeof(MissionController), "Invalid Level")]
    internal class when_an_invalid_level_number_is_specified : with_mvc_controller<MissionController>
        {
        private Establish context = () => ControllerUnderTest = ContextBuilder
            .WithMission().WithId(1).Level(1).BuildMission()
            .Build();
        private Because of = () => result = (HttpStatusCodeResult) ControllerUnderTest.Level(1, InvalidLevel);
        private It should_return_404 = () => result.StatusCode.ShouldEqual(404);
        private static HttpStatusCodeResult result;
        private const int InvalidLevel = 2;
        }

    internal class when_valid_mission_and_level_ids_are_specified : with_mvc_controller<MissionController>
        {
        private Establish context = () => ControllerUnderTest = ContextBuilder
            .WithMission().WithId(1).Level(1)
            .WithTrack(1).BuildTrack()
            .WithTrack(2).BuildTrack()
            .WithTrack(3).BuildTrack()
            .BuildMission()
            .Build();
        private Because of = () =>
            {
            result = (ViewResult) ControllerUnderTest.Level(1, 1);
            model = result.Model as MissionProgressViewModel;
            };
        private It should_have_expected_level = () => model.Level.ShouldEqual(1);
        private It should_have_expected_track_count = () => model.Tracks.Count().ShouldEqual(3);
        private static ViewResult result;
        private static MissionProgressViewModel model;
        }
    }