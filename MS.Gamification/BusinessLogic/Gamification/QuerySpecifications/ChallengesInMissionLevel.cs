// This file is part of the MS.Gamification project
// 
// File: ChallengesInMissionLevel.cs  Created: 2016-08-20@00:25
// Last modified: 2016-08-20@03:34

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications
    {
    /// <summary>
    ///     Returns all of the challenges associated with a mission Level.
    /// </summary>
    /// <seealso cref="Challenge" />
    public class ChallengesInMissionLevel : QuerySpecification<Challenge>
        {
        private readonly int levelId;

        public ChallengesInMissionLevel(int levelId)
            {
            this.levelId = levelId;
            }

        public override IQueryable<Challenge> GetQuery(IQueryable<Challenge> items)
            {
            var query = from challenge in items
                        where challenge.MissionTrack.MissionLevelId == levelId
                        select challenge;
            return query;
            }
        }
    }