// This file is part of the MS.Gamification project
// 
// File: ObservationsForChallenge.cs  Created: 2016-07-24@09:25
// Last modified: 2016-07-24@12:15

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    public class ObservationsForChallenge : QuerySpecification<Observation>
        {
        private readonly int challengeId;
        private readonly string userId;

        public ObservationsForChallenge(string userId, int challengeId)
            {
            this.userId = userId;
            this.challengeId = challengeId;
            }

        public override IQueryable<Observation> GetQuery(IQueryable<Observation> items)
            {
            var query = from observation in items
                        where observation.UserId == userId
                        where observation.ChallengeId == challengeId
                        select observation;
            return query;
            }
        }
    }