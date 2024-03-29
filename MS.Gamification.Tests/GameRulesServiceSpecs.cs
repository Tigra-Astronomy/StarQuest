﻿// This file is part of the MS.Gamification project
// 
// File: GameRulesServiceSpecs.cs  Created: 2016-07-09@20:14
// Last modified: 2016-07-09@22:26

using System.Linq;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.GameLogic;
using MS.Gamification.Models;
using MS.Gamification.Tests.Controllers;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests
    {
    [Subject(typeof(GameRulesService))]
    class when_testing_if_a_level_is_complete_and_there_are_no_observations
        : with_standard_mission<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder.Build();
        Because of = () => result = RulesService.IsLevelComplete(UnitOfWork.MissionLevels.Get(1), Enumerable.Empty<Observation>());
        It should_be_false = () => result.ShouldBeFalse();
        static bool result;
        }

    [Subject(typeof(GameRulesService))]
    class when_testing_if_a_level_is_complete_and_there_are_sufficient_observations
        : with_standard_mission<MissionController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithStandardUser("user", "Joe User")
            .WithObservation().ForUserId("user").ForChallenge(100).Approved().BuildObservation()
            .WithObservation().ForUserId("user").ForChallenge(101).Approved().BuildObservation()
            .Build();
        Because of = () =>
            result = RulesService.IsLevelComplete(UnitOfWork.MissionLevels.Get(1), UnitOfWork.Observations.GetAll());
        It should_be_true = () => result.ShouldBeTrue();
        static bool result;
        }
    }