// This file is part of the MS.Gamification project
// 
// File: ChallengesInMissionSpecs.cs  Created: 2016-07-04@18:17
// Last modified: 2016-07-04@19:26

using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using MS.Gamification.Controllers;
using MS.Gamification.Models;
using MS.Gamification.Tests.Controllers;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.QuerySpecifications
    {
    class when_querying_the_challenges_for_a_mission : with_standard_mission<ChallengeController>
        {
        Establish context = () => ControllerUnderTest = ContextBuilder
            .WithMission().Level(2).WithId(99)
            .WithTrack(1).WithChallenge("Not counted").BuildChallenge().BuildTrack()
            .BuildMission()
            .Build();
        // Standard Mission has ID=1
        Because of = () => results = UnitOfWork.Challenges.AllSatisfying(new ChallengesInMission(1));
        It should_find_only_challenges_for_mission_1 = () => results.Count().ShouldEqual(6);
        static IEnumerable<Challenge> results;
        }
    }