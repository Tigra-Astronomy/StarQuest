﻿// This file is part of the MS.Gamification project
// 
// File: ObservationsForUserMissionSpecs.cs  Created: 2016-07-02@21:32
// Last modified: 2016-07-03@00:38

using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.GameLogic.QuerySpecifications;
using MS.Gamification.Models;
using MS.Gamification.Tests.Controllers;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.QuerySpecifications
    {
    [Subject(typeof(ObservationsForUserMission))]
    class when_fetching_observations_for_a_users_who_has_observed_multiple_missions : with_standard_mission<ObservationController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithStandardUser("user", "Joe User")
            .WithStandardUser("otherUser", "Anonymous")
            .WithObservation().ForChallenge(100).ForUserId("user").BuildObservation()
            .WithObservation().ForChallenge(200).ForUserId("user").BuildObservation()
            .WithObservation().ForChallenge(200).ForUserId("otherUser").BuildObservation()
            .Build();
        Because of = () =>
            results = UnitOfWork.Observations.AllSatisfying(new ObservationsForUserMission("user", 1));
        It should_fetch_two_observations = () => results.Count().ShouldEqual(2);
        static IEnumerable<Observation> results;
        }
    }