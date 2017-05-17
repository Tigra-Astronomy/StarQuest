using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications
    {
    public class MissionTrackWithBadgeAndLevel : QuerySpecification<MissionTrack>
        {
        public MissionTrackWithBadgeAndLevel()
            {
            FetchStrategy.Include(p => p.Badge);
            FetchStrategy.Include(p => p.MissionLevel);
            }
        public override IQueryable<MissionTrack> GetQuery(IQueryable<MissionTrack> items)
            {
            return items;
            }
        }
    }