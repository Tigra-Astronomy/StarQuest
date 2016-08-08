// This file is part of the MS.Gamification project
// 
// File: MissionLevelsSpecs.cs  Created: 2016-08-07@17:21
// Last modified: 2016-08-09@00:01

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
     * Behaviours for mission levels
     * 
     * Deleting a level
     * When there are observations associated with the level
     * + Should throw
     * + Should not delete the level
     * When there are no associated observations
     * + Should delete the level from the database
     * 
     * Creating a new level
     * When the level number is unique within the associated mission
     *  + It should create the level
     * When the level number already exists within the associated mission
     *  + It should throw
     *  + The level should not be created
     * 
     * Updating; Changing the associated mission
     * When there are associated observations
     * + Should throw
     * + Should not update the level
     * When the target mission already has the same level number
     * + Should throw
     * + Should not update the level
     * When there are no associated observations and the level number is unique
     * + The change should succeed
     */

    [Subject(typeof(GameRulesService), "Delete level")]
    class when_deleting_a_mission_level_with_associated_observations : with_standard_mission<DummyGameController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithStandardUser("user", "Test User")
            .WithObservation().Approved().ForChallenge(100).ForUserId("user").BuildObservation()
            .Build();
        Because of = () => exception = Catch.Exception(() => RulesService.DeleteLevelAsync(1).Await());
        It should_throw = () => exception.ShouldBeOfExactType<InvalidOperationException>();
        It should_not_delete_the_level = () => UnitOfWork.MissionLevels.GetAll().Count(p => p.Level == 1).ShouldEqual(1);
        static Exception exception;
        }

    [Subject(typeof(GameRulesService), "Delete level")]
    class when_deleting_a_mission_level : with_standard_mission<DummyGameController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .Build();
        Because of = () => RulesService.DeleteLevelAsync(1).Await();
        It should_delete_the_level = () => UnitOfWork.MissionLevels.GetAll().Any(p => p.Id == 1).ShouldBeFalse();
        static Exception exception;
        }

    [Subject(typeof(GameRulesService), "Create level")]
    class when_creating_a_mission_level_with_a_unique_level_number : with_standard_mission<DummyGameController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .Build();
            newLevel = new MissionLevel
                {
                Name = "Level 99",
                AwardTitle = "Persistence",
                Level = 99,
                MissionId = 1
                };
            };
        Because of = () => RulesService.CreateLevelAsync(newLevel).Await();
        It should_create_the_level = () => UnitOfWork.MissionLevels.GetAll().Single(p => p.Level == 99);
        static Exception exception;
        static MissionLevel newLevel;
        }

    [Subject(typeof(GameRulesService), "Create level")]
    class when_creating_a_mission_level_with_a_non_unique_level_number : with_standard_mission<DummyGameController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .Build();
            newLevel = new MissionLevel
                {
                Name = "Level 99",
                AwardTitle = "Persistence",
                Level = 1,
                MissionId = 1
                };
            };
        Because of = () => exception = Catch.Exception(() => RulesService.CreateLevelAsync(newLevel).Await());
        It should_throw = () => exception.ShouldBeOfExactType<InvalidOperationException>();
        It should_not_create_the_duplicate_level = () => UnitOfWork.MissionLevels.GetAll().Count(p => p.Level == 1).ShouldEqual(1);
        static Exception exception;
        static MissionLevel newLevel;
        }

    [Subject(typeof(GameRulesService), "Change mission association")]
    class when_editing_the_mission_association_and_game_rules_are_satisfied : with_standard_mission<DummyGameController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .WithEntity(new Mission {Id = 2, Title = "Second Mission"})
                .Build();
            originalLevel = UnitOfWork.MissionLevels.Get(1);
            updatedLevel = Mapper.Map<MissionLevel, MissionLevel>(originalLevel);
            updatedLevel.MissionId = 2;
            };
        Because of = () => RulesService.UpdateLevelAsync(updatedLevel).Await();
        It should_succeed = () => UnitOfWork.MissionLevels.Get(originalLevel.Id).MissionId.ShouldEqual(2);
        static MissionLevel updatedLevel;
        static MissionLevel originalLevel;
        }

    [Subject(typeof(GameRulesService), "Change mission association")]
    class when_editing_the_mission_association_and_the_target_mission_already_has_that_level
        : with_standard_mission<DummyGameController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .WithMissionLevel(2).Level(1).BuildMission()
                .Build();
            originalLevel = UnitOfWork.MissionLevels.GetAll().Single(p => p.Level == 1 && p.MissionId == 1);
            updatedLevel = Mapper.Map<MissionLevel, MissionLevel>(originalLevel);
            updatedLevel.MissionId = 2;
            };
        Because of = () => Exception = Catch.Exception(() => { RulesService.UpdateLevelAsync(updatedLevel).Await(); });
        It should_throw = () => Exception.ShouldBeOfExactType<InvalidOperationException>();
        It should_not_update_the_level = () => UnitOfWork.MissionLevels.Get(originalLevel.Id).MissionId.ShouldEqual(1);
        static MissionLevel updatedLevel;
        static MissionLevel originalLevel;
        static Exception Exception;
        }

    [Subject(typeof(GameRulesService), "Change mission association")]
    class when_editing_the_mission_association_and_there_are_associated_observations
        : with_standard_mission<DummyGameController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .WithStandardUser("user", "Test User")
                .WithObservation().ForChallenge(100).ForUserId("user").BuildObservation()
                .WithEntity(new Mission {Id = 2, Title = "Mission 2"})
                .Build();
            originalLevel = UnitOfWork.MissionLevels.GetAll().Single(p => p.Level == 1 && p.MissionId == 1);
            updatedLevel = Mapper.Map<MissionLevel, MissionLevel>(originalLevel);
            updatedLevel.MissionId = 2;
            };
        Because of = () => Exception = Catch.Exception(() => { RulesService.UpdateLevelAsync(updatedLevel).Await(); });
        It should_throw = () => Exception.ShouldBeOfExactType<InvalidOperationException>();
        It should_not_update_the_level = () => UnitOfWork.MissionLevels.Get(originalLevel.Id).MissionId.ShouldEqual(1);
        static MissionLevel updatedLevel;
        static MissionLevel originalLevel;
        static Exception Exception;
        }
    }