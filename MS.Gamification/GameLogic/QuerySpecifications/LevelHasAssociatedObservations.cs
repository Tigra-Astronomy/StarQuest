// This file is part of the MS.Gamification project
// 
// File: LevelHasAssociatedObservations.cs  Created: 2016-08-08@20:21
// Last modified: 2016-08-08@20:37

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    /// <summary>
    ///     A query specification that can be used to determine whether a given Mission Level has any observations
    ///     associated with it. The query returns a set of integers, which will be empty if no observations were found,
    ///     or will contain a single element representing the number of observations associated with the level. This is
    ///     designed to be convenient for use with <c>Repository.GetMaybe</c>.
    /// </summary>
    public class LevelHasAssociatedObservations : QuerySpecification<Observation, int>
        {
        private readonly int levelId;

        public LevelHasAssociatedObservations(int levelId)
            {
            this.levelId = levelId;
            }

        public override IQueryable<int> GetQuery(IQueryable<Observation> items)
            {
            /*
             * Returns a collection of counts, where each count is the number of observations
             * associated with a mission level. Since the observations are pre-filtered to only
             * those in a single mission level, then the result will either be a single element
             * containing the count of the associated observations, or it will be an empty set
             * if none were found. This can be used directly with IRepository.GetMaybe().
             */
            var query = from observation in items
                        let associatedLevel = observation.Challenge.MissionTrack.MissionLevelId
                        where associatedLevel == levelId
                        group observation by associatedLevel
                        into associatedObservations
                        select associatedObservations.Count();
            return query;
            }
        }
    }