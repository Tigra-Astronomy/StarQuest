// This file is part of the MS.Gamification project
// 
// File: MissionControllerSpecs.cs  Created: 2016-07-01@07:36
// Last modified: 2016-07-04@01:09

using System.Linq;
using System.Web.Mvc;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.Models;
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
 *      + Should return a 404 Not Found
 * 
 * Given: 
 *      A standard mission ID=1 Level=1 (see context base with_standard_mission)
 *      A logged in user with the following observations:
 *          Saw the New Moon
 *          Saw Jupiter
 *          Saw M45
 * When the mission progress view is created
 * Then
 *      The View model should meet the following specifications:
 *      - should show that the mission is 50% complete overall
 *      - should show that each of the tracks is 50% complete
 *      - should mark challenges with matching observations as completed
 *      - should not mark challenges without any observations as completed
 *     
 */

namespace MS.Gamification.Tests.Controllers
    {
    [Subject(typeof(MissionController), "Level ID omitted")]
    class when_invoking_the_level_action_with_no_id_parameter : with_mvc_controller<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithMission().WithId(1).BuildMission()
            .Build();
        Because of = () => result = (ViewResult) ControllerUnderTest.Level(1, null);
        It should_assume_level_1 = () => ((MissionProgressViewModel) result.Model).Level.ShouldEqual(1);
        static ViewResult result;
        }

    [Subject(typeof(MissionController), "Invalid Mission ID")]
    class when_an_invalid_mission_id_is_specified : with_mvc_controller<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithMission().BuildMission()
            .Build();
        Because of = () => result = (HttpStatusCodeResult) ControllerUnderTest.Level(InvalidMissionId, null);
        It should_return_404 = () => result.StatusCode.ShouldEqual(404);
        static HttpStatusCodeResult result;
        const int InvalidMissionId = 99;
        }

    [Subject(typeof(MissionController), "Invalid Level")]
    class when_the_mission_id_is_valid_but_an_invalid_level_number_is_specified
        : with_mvc_controller<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithMission().WithId(1).Level(1).BuildMission()
            .Build();
        Because of = () => result = (HttpStatusCodeResult) ControllerUnderTest.Level(1, InvalidLevel);
        It should_return_404 = () => result.StatusCode.ShouldEqual(404);
        static HttpStatusCodeResult result;
        const int InvalidLevel = 2;
        }

    class when_valid_mission_and_level_ids_are_specified : with_standard_mission<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () =>
            {
            result = (ViewResult) ControllerUnderTest.Level(1, 1);
            model = result.Model as MissionProgressViewModel;
            tracks = model.Tracks.ToArray();
            };
        It should_have_expected_level = () => model.Level.ShouldEqual(1);
        It should_have_expected_track_count = () => model.Tracks.Count().ShouldEqual(3);
        It should_have_expected_challenges_in_track_1 = () => tracks[0].Challenges.Count.ShouldEqual(2);
        It should_have_expected_challenges_in_track_2 = () => tracks[1].Challenges.Count.ShouldEqual(2);
        It should_have_expected_challenges_in_track_3 = () => tracks[2].Challenges.Count.ShouldEqual(2);
        static ViewResult result;
        static MissionTrack[] tracks;
        static MissionProgressViewModel model;
        }

    class when_the_user_has_completed_one_challenge_in_each_track : with_standard_mission<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithStandardUser("user", "Joe User")
            .WithRequestingUser("user", "Joe User")
            .WithObservation().ForUserId("user").ForChallenge(100).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(200).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(300).BuildObservation()
            .Build();
        Because of = () => Model = ((ViewResult) ControllerUnderTest.Level(1, 1)).Model as MissionProgressViewModel;
        It should_have_the_expected_overall_progress = () => Model.OverallProgressPercent.ShouldEqual(50);
        static MissionProgressViewModel Model;
        }
    }