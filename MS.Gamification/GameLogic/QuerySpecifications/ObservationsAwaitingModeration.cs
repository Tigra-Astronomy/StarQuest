// This file is part of the MS.Gamification project
// 
// File: ObservationsAwaitingModeration.cs  Created: 2016-05-19@01:02
// Last modified: 2016-05-19@01:36

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    public class ObservationsAwaitingModeration : QuerySpecification<Observation>
        {
        public ObservationsAwaitingModeration()
            {
            FetchStrategy.Include(p => p.User).Include(p => p.Challenge);
            }

        public override IQueryable<Observation> GetQuery(IQueryable<Observation> items)
            {
            var query = from item in items
                        where item.Status == ModerationState.AwaitingModeration
                        select item;
            return query;
            }
        }
    }