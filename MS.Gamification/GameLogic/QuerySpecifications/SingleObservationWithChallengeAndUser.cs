// This file is part of the MS.Gamification project
// 
// File: SingleObservationWithChallengeAndUser.cs  Created: 2016-07-09@20:14
// Last modified: 2016-07-28@10:48

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    public class SingleObservationWithChallengeAndUser : QuerySpecification<Observation>
        {
        private readonly int observationId;

        public SingleObservationWithChallengeAndUser(int observationId)
            {
            this.observationId = observationId;
            FetchStrategy.Include(p => p.Challenge);
            FetchStrategy.Include(p => p.User);
            }

        public override IQueryable<Observation> GetQuery(IQueryable<Observation> items)
            {
            return from item in items
                   where item.Id == observationId
                   select item;
            }
        }
    }