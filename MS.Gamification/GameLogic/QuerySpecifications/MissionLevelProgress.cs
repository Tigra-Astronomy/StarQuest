// This file is part of the MS.Gamification project
// 
// File: MissionLevelProgress.cs  Created: 2016-07-09@20:14
// Last modified: 2016-07-22@13:37

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    public class MissionLevelProgress : QuerySpecification<Mission>
        {
        private readonly int missionId;

        public MissionLevelProgress(int missionId)
            {
            this.missionId = missionId;
            FetchStrategy.Include(p => p.MissionLevels);
            FetchStrategy.Include("MissionLevels.Tracks");
            FetchStrategy.Include("MissionLevels.Tracks.Badge");
            FetchStrategy.Include("MissionLevels.Tracks.Challenges");
            FetchStrategy.Include("MissionLevels.Tracks.Challenges.Category");
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