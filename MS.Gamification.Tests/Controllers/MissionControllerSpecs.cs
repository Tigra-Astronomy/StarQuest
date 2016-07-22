// This file is part of the MS.Gamification project
// 
// File: MissionControllerSpecs.cs  Created: 2016-07-09@20:14
// Last modified: 2016-07-22@10:11

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
 *      + The returned View should be based on Progress 1
 * When the mission ID is invalid
 *      + Should return a 404 Not Found error
 * When the mission ID is valid but the Progress number is > 1
 *      + Should return a 404 Not Found
 * 
 * Given: 
 *      A standard mission ID=1 Progress=1 (see context base with_standard_mission)
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
    [Subject(typeof(MissionController), "Invalid Mission ID")]
    class when_an_invalid_mission_id_is_specified : with_mvc_controller<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithMissionLevel().BuildMission()
            .Build();
        Because of = () => result = (HttpStatusCodeResult) ControllerUnderTest.Progress(InvalidMissionId);
        It should_return_404 = () => result.StatusCode.ShouldEqual(404);
        static HttpStatusCodeResult result;
        const int InvalidMissionId = 99;
        }

    [Ignore("Controller no longer takes a levelId argument")]
    [Subject(typeof(MissionController), "Invalid Progress")]
    class when_the_mission_id_is_valid_but_an_invalid_level_number_is_specified
        : with_mvc_controller<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithMissionLevel().WithId(1).Level(1).BuildMission()
            .Build();
        Because of = () => result = (HttpStatusCodeResult) ControllerUnderTest.Progress(1 /*, InvalidLevel*/);
        It should_return_404 = () => result.StatusCode.ShouldEqual(404);
        static HttpStatusCodeResult result;
        const int InvalidLevel = 2;
        }

    [Subject(typeof(MissionController), "observations retrieval")]
    class when_valid_mission_and_level_ids_are_specified : with_standard_mission<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () =>
            {
            result = (ViewResult) ControllerUnderTest.Progress(1);
            model = result.Model as MissionProgressViewModel;
            tracks = model.Levels.First().Tracks.ToArray();
            };
        It should_have_expected_level_count = () => model.Levels.Count.ShouldEqual(1);
        It should_have_expected_track_count = () => tracks.Count().ShouldEqual(3);
        It should_have_expected_challenges_in_track_1 = () => tracks[0].Challenges.Count.ShouldEqual(2);
        It should_have_expected_challenges_in_track_2 = () => tracks[1].Challenges.Count.ShouldEqual(2);
        It should_have_expected_challenges_in_track_3 = () => tracks[2].Challenges.Count.ShouldEqual(2);
        static ViewResult result;
        static MissionTrack[] tracks;
        static MissionProgressViewModel model;
        }

    [Subject(typeof(MissionController), "Mission Progress computation")]
    class when_the_user_has_completed_one_challenge_in_each_track : with_standard_mission<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithStandardUser("user", "Joe User")
            .WithRequestingUser("user", "Joe User")
            .WithObservation().ForUserId("user").ForChallenge(100).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(200).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(300).BuildObservation()
            .Build();
        Because of = () => Model = ((ViewResult) ControllerUnderTest.Progress(1)).Model as MissionProgressViewModel;
        It should_have_the_expected_overall_progress = () => Model.Levels.First().OverallProgressPercent.ShouldEqual(50);
        static MissionProgressViewModel Model;
        }

    [Subject(typeof(MissionController), "Mission Progress computation")]
    class when_the_user_has_completed_all_challenges_in_all_tracks : with_standard_mission<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithStandardUser("user", "Joe User")
            .WithRequestingUser("user", "Joe User")
            .WithObservation().ForUserId("user").ForChallenge(100).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(101).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(200).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(201).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(300).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(400).BuildObservation()
            .Build();
        Because of = () => Model = ((ViewResult) ControllerUnderTest.Progress(1)).Model as MissionProgressViewModel;
        It should_have_100_percent_overall_progress = () => Model.Levels.First().OverallProgressPercent.ShouldEqual(100);
        static MissionProgressViewModel Model;
        }

    [Subject(typeof(MissionController), "Mission Progress computation")]
    class when_the_user_has_submitted_more_observations_than_required_for_some_challenges
        : with_standard_mission<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithStandardUser("user", "Joe User")
            .WithRequestingUser("user", "Joe User")
            .WithObservation().ForUserId("user").ForChallenge(100).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(100).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(200).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(200).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(300).BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(300).BuildObservation()
            .Build();
        Because of = () => Model = ((ViewResult) ControllerUnderTest.Progress(1)).Model as MissionProgressViewModel;
        It should_only_count_the_first_valid_observation_for_each_challenge =
            () => Model.Levels.First().OverallProgressPercent.ShouldEqual(50);
        static MissionProgressViewModel Model;
        }

    [Subject(typeof(MissionController), "Track Progress computation")]
    class when_computing_progress_for_each_track
        : with_standard_mission<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithStandardUser("user", "Joe User")
            .WithRequestingUser("user", "Joe User")
            .WithObservation().ForUserId("user").ForChallenge(200).BuildObservation() // Track 2 = 50%
            .WithObservation().ForUserId("user").ForChallenge(300).BuildObservation() // Track 3
            .WithObservation().ForUserId("user").ForChallenge(400).BuildObservation() // Track 3 = 100%
            .Build();
        Because of = () => Model = ((ViewResult) ControllerUnderTest.Progress(1)).Model as MissionProgressViewModel;
        It should_compute_progress_for_track_1 = () => Model.Levels[0].TrackProgress[0].ShouldEqual(0);
        It should_compute_progress_for_track_2 = () => Model.Levels[0].TrackProgress[1].ShouldEqual(50);
        It should_compute_progress_for_track_3 = () => Model.Levels[0].TrackProgress[2].ShouldEqual(100);
        It should_compute_overall_progress = () => Model.Levels[0].OverallProgressPercent.ShouldEqual(50);
        static MissionProgressViewModel Model;
        }

    [Subject(typeof(MissionController), "Track Progress computation")]
    class when_computing_progress_for_each_track_and_there_are_unapproved_observations
        : with_standard_mission<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithStandardUser("user", "Joe User")
            .WithRequestingUser("user", "Joe User")
            .WithObservation().ForUserId("user").ForChallenge(100).AwaitingModeration().BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(200).Rejected().BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(300).Approved().BuildObservation()
            .Build();
        Because of = () => Model = ((ViewResult) ControllerUnderTest.Progress(1)).Model as MissionProgressViewModel;
        It should_compute_progress_for_track_1 = () => Model.Levels[0].TrackProgress[0].ShouldEqual(0);
        It should_compute_progress_for_track_2 = () => Model.Levels[0].TrackProgress[1].ShouldEqual(0);
        It should_compute_progress_for_track_3 = () => Model.Levels[0].TrackProgress[2].ShouldEqual(50);
        It should_compute_overall_progress = () => Model.Levels[0].OverallProgressPercent.ShouldEqual(100 / 6);
        static MissionProgressViewModel Model;
        }
    }