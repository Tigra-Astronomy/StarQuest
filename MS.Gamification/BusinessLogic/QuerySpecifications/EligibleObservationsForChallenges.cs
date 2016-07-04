// This file is part of the MS.Gamification project
// 
// File: EligibleObservationsForChallenges.cs  Created: 2016-07-04@20:37
// Last modified: 2016-07-04@21:06

using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QuerySpecifications
    {
    class EligibleObservationsForChallenges : QuerySpecification<Observation>
        {
        private readonly IEnumerable<Challenge> challenges;
        private readonly string userId;

        public EligibleObservationsForChallenges(IEnumerable<Challenge> challenges, string userId)
            {
            this.challenges = challenges;
            this.userId = userId;
            }

        public override IQueryable<Observation> GetQuery(IQueryable<Observation> items)
            {
            var approvedObservationsForUser = from observation in items
                                              where observation.UserId == userId
                                              where observation.Status == ModerationState.Approved
                                              orderby observation.ObservationDateTimeUtc ascending
                                              select observation;
            var eligibleObservations = from challenge in challenges
                                       join observation in approvedObservationsForUser on challenge.Id equals
                                           observation.ChallengeId
                                       select observation;
            return eligibleObservations.DistinctBy(p => p.ChallengeId).AsQueryable();
            }
        }
    }