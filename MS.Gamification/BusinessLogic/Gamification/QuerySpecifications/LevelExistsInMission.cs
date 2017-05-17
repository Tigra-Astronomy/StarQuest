// This file is part of the MS.Gamification project
// 
// File: LevelExistsInMission.cs  Created: 2016-08-08@22:19
// Last modified: 2016-08-08@22:33

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications
    {
    public class LevelExistsInMission : QuerySpecification<MissionLevel, int>
        {
        private readonly int levelNumber;
        private readonly int missionId;

        public LevelExistsInMission(int levelNumber, int missionId)
            {
            this.levelNumber = levelNumber;
            this.missionId = missionId;
            }

        public override IQueryable<int> GetQuery(IQueryable<MissionLevel> items)
            {
            var query = from level in items
                        where level.MissionId == missionId
                        where level.Level == levelNumber
                        select level.Level;
            return query;
            }
        }
    }