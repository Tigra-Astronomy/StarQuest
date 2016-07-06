// This file is part of the MS.Gamification project
// 
// File: MissionLevelProgress.cs  Created: 2016-07-01@20:21
// Last modified: 2016-07-05@00:40

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QuerySpecifications
    {
    public class MissionLevelProgress : QuerySpecification<MissionLevel>
        {
        private readonly int levelId;
        private readonly int missionId;

        public MissionLevelProgress(int missionId, int levelId = 1)
            {
            this.missionId = missionId;
            this.levelId = levelId;
            FetchStrategy.Include(p => p.Tracks);
            }

        public override IQueryable<MissionLevel> GetQuery(IQueryable<MissionLevel> items)
            {
            var query = from item in items
                        where item.Id == missionId
                        where item.Level == levelId
                        select item;
            return query;
            }
        }
    }