// This file is part of the MS.Gamification project
// 
// File: MissionLevelProgress.cs  Created: 2016-07-09@20:14
// Last modified: 2016-08-11@07:14

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications
    {
    public class MissionLevelProgress : QuerySpecification<Mission>
        {
        private readonly int missionId;

        public MissionLevelProgress(int missionId)
            {
            this.missionId = missionId;
            FetchStrategy.Include(p => p.MissionLevels);
            FetchStrategy.Include(p => p.MissionLevels.Select(q => q.Tracks.Select(r => r.Badge)));
            FetchStrategy.Include(p => p.MissionLevels.Select(q => q.Tracks.Select(r => r.Challenges.Select(s => s.Category))));
            }

        public override IQueryable<Mission> GetQuery(IQueryable<Mission> items)
            {
            var query = from item in items
                        where item.Id == missionId
                        select item;
            return query;
            }
        }
    }