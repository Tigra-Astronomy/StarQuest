// This file is part of the MS.Gamification project
// 
// File: MissionProgressSummary.cs  Created: 2016-07-29@19:46
// Last modified: 2016-07-29@19:51

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    public class MissionProgressSummary : QuerySpecification<Mission>
        {
        public MissionProgressSummary()
            {
            FetchStrategy.Include(p => p.MissionLevels);
            FetchStrategy.Include("MissionLevels.Tracks");
            FetchStrategy.Include("MissionLevels.Tracks.Challenges");
            }

        public override IQueryable<Mission> GetQuery(IQueryable<Mission> items)
            {
            return items;
            }
        }
    }