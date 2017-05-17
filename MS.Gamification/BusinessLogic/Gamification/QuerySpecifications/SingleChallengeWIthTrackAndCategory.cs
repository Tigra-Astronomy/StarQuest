// This file is part of the MS.Gamification project
// 
// File: SingleChallengeWIthTrackAndCategory.cs  Created: 2016-08-19@03:05
// Last modified: 2016-08-19@03:09

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications
    {
    public class SingleChallengeWithTrackAndCategory : QuerySpecification<Challenge>
        {
        private readonly int challengeId;

        public SingleChallengeWithTrackAndCategory(int challengeId)
            {
            this.challengeId = challengeId;
            FetchStrategy
                .Include(p => p.MissionTrack)
                .Include(p => p.Category);
            }

        public override IQueryable<Challenge> GetQuery(IQueryable<Challenge> items)
            {
            return from challenge in items
                   where challenge.Id == challengeId
                   select challenge;
            }
        }
    }