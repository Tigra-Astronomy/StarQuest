using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QuerySpecifications
    {
    public class MissionLevelProgress : QuerySpecification<Mission>
        {
        readonly int levelId;
        readonly int missionId;

        public MissionLevelProgress(int missionId, int levelId)
            {
            this.missionId = missionId;
            this.levelId = levelId;
            FetchStrategy.Include(p => p.Tracks);
            }

        public override IQueryable<Mission> GetQuery(IQueryable<Mission> items)
            {
            var query = from item in items
                        where item.Id == missionId
                        where item.Level == levelId
                        select item;
            return query;
            }
        }
    }