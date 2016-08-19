// This file is part of the MS.Gamification project
// 
// File: TrackHasAssociatedObservations.cs  Created: 2016-08-13@21:36
// Last modified: 2016-08-13@21:38

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    public class TrackHasAssociatedObservations : QuerySpecification<Observation, int>
        {
        private readonly int trackId;

        public TrackHasAssociatedObservations(int trackId)
            {
            this.trackId = trackId;
            }

        public override IQueryable<int> GetQuery(IQueryable<Observation> items)
            {
            /*
             * Returns a collection of counts, where each count is the number of observations
             * associated with a mission track. Since the observations are pre-filtered to only
             * those in a single track, then the result will either be a single element
             * containing the count of the associated observations, or it will be an empty set
             * if none were found. This can be used directly with IRepository.GetMaybe().
             */
            var query = from observation in items
                        let associatedTrack = observation.Challenge.MissionTrack.Id
                        where associatedTrack == trackId
                        group observation by associatedTrack
                        into associatedObservations
                        select associatedObservations.Count();
            return query;
            }
        }
    }