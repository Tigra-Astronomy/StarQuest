// This file is part of the MS.Gamification project
// 
// File: MissionHasAssociatedObservations.cs  Created: 2016-08-06@12:18
// Last modified: 2016-08-08@20:21

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    public class MissionHasAssociatedObservations : QuerySpecification<Observation, int>
        {
        private readonly int missionId;

        public MissionHasAssociatedObservations(int missionId)
            {
            this.missionId = missionId;
            }

        public override IQueryable<int> GetQuery(IQueryable<Observation> items)
            {
            /*
             * Returns a collection of counts, where each count is the number of observations associated
             * with a mission. Since the observations are pre-filtered to only those in a single mission,
             * then the result will either be a single element containing the count of the associated
             * observations, or it will be an empty set if none were found. This can be used with GetMaybe.
             */
            var query = from observation in items
                        let associatedMission = observation.Challenge.MissionTrack.MissionLevel.MissionId
                        where associatedMission == missionId
                        group observation by associatedMission
                        into associatedObservations
                        select associatedObservations.Count();
            return query;
            }
        }
    }