using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QuerySpecifications
    {
    /// <summary>
    ///     Specifies the set of observations submitted by a given user against the given Mission ID. Eagerly loads the
    ///     Challenge and Mission Track for each observation.
    /// </summary>
    /// <seealso cref="BusinessLogic.QuerySpecification{Observation}" />
    public class ObservationsForUserMission : QuerySpecification<Observation>
        {
        private readonly int missionId;
        private readonly string userId;

        public ObservationsForUserMission(string userId, int missionId)
            {
            this.userId = userId;
            this.missionId = missionId;
            FetchStrategy.Include(p => p.Challenge.MissionTrack);
            }

        public override IQueryable<Observation> GetQuery(IQueryable<Observation> items)
            {
            var observations = from observation in items
                               where observation.UserId == userId
                               where observation.Challenge.MissionTrack.MissionLevelId == missionId
                               orderby observation.ObservationDateTimeUtc ascending
                               select observation;
            return observations;
            }
        }
    }