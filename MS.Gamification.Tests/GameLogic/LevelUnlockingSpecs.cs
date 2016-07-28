// This file is part of the MS.Gamification project
// 
// File: LevelUnlockingSpecs.cs  Created: 2016-07-26@09:12
// Last modified: 2016-07-28@18:10

using System;
using Machine.Specifications;
using MS.Gamification.GameLogic;
using MS.Gamification.Tests.Controllers;
using MS.Gamification.Tests.TestHelpers;
using MS.Gamification.ViewModels.Mission;

namespace MS.Gamification.Tests.GameLogic
    {
    [Subject(typeof(GameRulesService), "Level unlocking")]
    class when_evaluating_level_preconditions_and_there_are_no_rules : with_standard_mission<DummyGameController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .WithStandardUser("user", "Joe User")
                .Build();

            Level = new LevelProgressViewModel
                {
                Precondition = string.Empty
                };
            };
        Because of = () => isUnlocked = RulesService.IsLevelUnlockedForUser(Level, "user");
        It should_be_unlocked = () => isUnlocked.ShouldBeTrue();
        static bool isUnlocked;
        static LevelProgressViewModel Level;
        }

    [Subject(typeof(GameRulesService), "Level unlocking")]
    class when_evaluating_level_preconditions_and_there_are_unsatisfied_rules : with_standard_mission<DummyGameController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .WithStandardUser("user", "Joe User")
                .Build();

            Level = new LevelProgressViewModel
                {
                Precondition = TestData.FromEmbeddedResource("PreconditionsEngine.HasAll-1-2-4.xml")
                };
            };
        Because of = () => isUnlocked = RulesService.IsLevelUnlockedForUser(Level, "user");
        It should_be_locked = () => isUnlocked.ShouldBeFalse();
        static bool isUnlocked;
        static LevelProgressViewModel Level;
        }

    [Subject(typeof(GameRulesService), "Level unlocking")]
    [Ignore("Effort.Extra limitation with many-to-many relationships")]
    class when_evaluating_level_preconditions_and_there_are_rules_which_are_satisfied : with_standard_mission<DummyGameController>
        {
        Establish context = () =>
            {
            ControllerUnderTest = ContextBuilder
                .WithUserAwardedBadges("user", "Joe User", 1, 2, 4)
                .Build();

            Level = new LevelProgressViewModel
                {
                Precondition = TestData.FromEmbeddedResource("PreconditionsEngine.HasAll-1-2-4.xml")
                };
            };
        Because of = () => isUnlocked = RulesService.IsLevelUnlockedForUser(Level, "user");
        It should_be_unlocked = () => isUnlocked.ShouldBeTrue();
        static bool isUnlocked;
        static LevelProgressViewModel Level;
        }

    [Subject(typeof(GameRulesService), "Level unlocking")]
    class when_evaluating_level_preconditions_and_something_throws : with_standard_mission<DummyGameController>
        {
        //ToDo - implement this test context
        Because of = () => { throw new NotImplementedException(); };
        It should_be_locked;
        }
    }