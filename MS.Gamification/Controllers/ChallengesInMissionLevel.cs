using System.Linq;
using MS.Gamification.GameLogic;
using MS.Gamification.Models;

namespace MS.Gamification.Controllers
    {
    public class ChallengesInMissionLevel : QuerySpecification<Challenge>
        {
        private readonly int missionId;

        public ChallengesInMissionLevel(int missionId)
            {
            this.missionId = missionId;
            }

        public override IQueryable<Challenge> GetQuery(IQueryable<Challenge> items)
            {
            var query = from challenge in items
                        where challenge.MissionTrack.MissionLevelId == missionId
                        select challenge;
            return query;
            }
        }
    }