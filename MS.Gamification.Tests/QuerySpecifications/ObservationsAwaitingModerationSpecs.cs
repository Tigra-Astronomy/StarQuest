﻿// This file is part of the MS.Gamification project
// 
// File: ObservationsAwaitingModerationSpecs.cs  Created: 2016-05-19@01:05
// Last modified: 2016-05-19@01:41

using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MS.Gamification.BusinessLogic.QuerySpecifications;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.QuerySpecifications
    {
    /*
     * Query Specification: Observations Awaiting Moderation
     * Given:
     *  A collection of observations, one of which is in Pending status
     * When:
     *  the query is run
     * Then:
     *  it should produce one result
     *  The one result should be in Pending status
     *  
     * When:
     *  There are no observations in Pending state
     * Then:
     *  It should produce an empty result
     */

    class when_querying_observations_and_there_is_one_observation_in_pending_state
        {
        Establish context = () => Observations = new List<Observation>
            {
            new Observation {Id = 1, Status = ModerationState.Approved},
            new Observation {Id = 2, Status = ModerationState.AwaitingModeration},
            new Observation {Id = 3, Status = ModerationState.Rejected}
            };
        Because of = () => Results = new ObservationsAwaitingModeration()
            .GetQuery(Observations.AsQueryable())
            .ToList();
        It should_produce_one_result = () => Results.Count.ShouldEqual(1);
        It should_be_awaiting_moderation = () => Results.Single().Status.ShouldEqual(ModerationState.AwaitingModeration);
        static List<Observation> Observations;
        static List<Observation> Results;
        }

    class when_querying_observations_and_there_are_no_observation_in_pending_state
        {
        Establish context = () => Observations = new List<Observation>
            {
            new Observation {Id = 1, Status = ModerationState.Approved},
            new Observation {Id = 3, Status = ModerationState.Rejected}
            };
        Because of = () => Results = new ObservationsAwaitingModeration()
            .GetQuery(Observations.AsQueryable())
            .ToList();
        It should_produce_no_results = () => Results.Count.ShouldEqual(0);
        static List<Observation> Observations;
        static List<Observation> Results;
        }
    }