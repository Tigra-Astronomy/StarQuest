// This file is part of the MS.Gamification project
// 
// File: MissionTrackSpecs.cs  Created: 2016-08-10@17:56
// Last modified: 2016-08-10@20:18

using System;
using System.Linq;
using Machine.Specifications;
using MS.Gamification.GameLogic;
using MS.Gamification.Models;
using MS.Gamification.Tests.Controllers;

namespace MS.Gamification.Tests.GameLogic
    {
    /*
     * Behaviours for Mission Track CRUD operations
     * 
     * Concern: Create new
     * When creating a new track, and there is already a track in the target mission with the same number
     * + It should throw
     * + It should not create the track
     * When creating a new track and it references a non-existent level
     * + It should throw
     * + It should not create the track
     * When creating a new track and it references a non-existent badge
     * - It should throw
     * - It should not create the track
     * When creating a new track with valid data
     * - It should add the track to the database
     * 
     */

    [Subject(typeof(GameRulesService), "Create New")]
    class when_adding_a_new_track_with_valid_data : with_standard_mission<DummyGameController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => RulesService.CreateTrackAsync(new MissionTrack
            {
            BadgeId = UnitOfWork.Badges.GetAll().First().Id,
            AwardTitle = "dummy",
            MissionLevelId = 1,
            Name = "Unit Test",
            Number = 99
            }).Await();
        It should_add_the_new_track_to_the_database =
            () => UnitOfWork.MissionTracks.GetAll().Count(p => p.Number == 99).ShouldEqual(1);
        }

    [Subject(typeof(GameRulesService), "Create New")]
    class when_adding_a_new_track_with_a_duplicate_track_number : with_standard_mission<DummyGameController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Exception = Catch.Exception(() =>
            {
            RulesService.CreateTrackAsync(new MissionTrack
                {
                BadgeId = UnitOfWork.Badges.GetAll().First().Id,
                AwardTitle = "dummy",
                MissionLevelId = 1,
                Name = "Unit Test",
                Number = 1
                }).Await();
            });
        It should_throw = () => Exception.ShouldBeOfExactType<InvalidOperationException>();
        It should_not_add_the_new_track_to_the_database =
            () => UnitOfWork.MissionTracks.GetAll().Any(p => p.AwardTitle == "dummy").ShouldBeFalse();
        static Exception Exception;
        }

    [Subject(typeof(GameRulesService), "Create New")]
    class when_adding_a_new_track_that_references_a_non_existent_level : with_standard_mission<DummyGameController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Exception = Catch.Exception(() =>
            {
            RulesService.CreateTrackAsync(new MissionTrack
                {
                BadgeId = UnitOfWork.Badges.GetAll().First().Id,
                AwardTitle = "dummy",
                MissionLevelId = 99,
                Name = "Unit Test",
                Number = 99
                }).Await();
            });
        It should_throw = () => Exception.ShouldBeOfExactType<InvalidOperationException>();
        It should_not_add_the_new_track_to_the_database =
            () => UnitOfWork.MissionTracks.GetAll().Any(p => p.AwardTitle == "dummy").ShouldBeFalse();
        static Exception Exception;
        }

    [Subject(typeof(GameRulesService), "Create New")]
    class when_adding_a_new_track_that_references_a_non_existent_badge : with_standard_mission<DummyGameController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => Exception = Catch.Exception(() =>
            {
            RulesService.CreateTrackAsync(new MissionTrack
                {
                BadgeId = UnitOfWork.Badges.GetAll().Max(p => p.Id) + 1,
                AwardTitle = "dummy",
                MissionLevelId = 1,
                Name = "Unit Test",
                Number = 99
                }).Await();
            });
        It should_throw = () => Exception.ShouldBeOfExactType<InvalidOperationException>();
        It should_not_add_the_new_track_to_the_database =
            () => UnitOfWork.MissionTracks.GetAll().Any(p => p.AwardTitle == "dummy").ShouldBeFalse();
        static Exception Exception;
        }
    }