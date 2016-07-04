using System.Linq;
using MS.Gamification.BusinessLogic;
using MS.Gamification.Models;

namespace MS.Gamification.Controllers
    {
    public class ChallengesInMission : QuerySpecification<Challenge>
        {
        private readonly int missionId;

        public ChallengesInMission(int missionId)
            {
            this.missionId = missionId;
            }

        public override IQueryable<Challenge> GetQuery(IQueryable<Challenge> items)
            {
            var query = from challenge in items
                        where challenge.MissionTrack.MissionId == missionId
                        select challenge;
            return query;
            }
        }
    }