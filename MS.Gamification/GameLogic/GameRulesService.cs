// This file is part of the MS.Gamification project
// 
// File: GameRulesService.cs  Created: 2016-07-08@02:12
// Last modified: 2016-07-08@03:23

using System;
using System.Collections.Generic;
using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic
    {
    public class GameRulesService
        {
        /// <summary>
        ///     Computes the percent complete for a set of challenges, given a set of eligible observations. The
        ///     computation is based on the number of points gained, rather than just a simple count.
        /// </summary>
        /// <param name="challenges">The set of challenges that represents 100% progress.</param>
        /// <param name="eligibleObservations">The eligible observations for the set of challenges.</param>
        /// <returns>The computed percentage, as an integer, guaranteed to be between 0% and 100% inclusive.</returns>
        /// <remarks>
        ///     It is assumed that the set of observations has already been filtered for eligibility, e.g. by calling
        ///     <see cref="EligibleObservations" />.
        /// </remarks>
        internal int ComputePercentComplete(IEnumerable<Challenge> challenges, IEnumerable<Observation> eligibleObservations)
            {
            var pointsAvailable = challenges.Select(p => p.Points).Sum();
            if (pointsAvailable < 1) return 0; // Avoid divide-by-zero error
            var pointsAwarded = eligibleObservations.Select(p => p.Challenge.Points).Sum();
            var percentComplete = pointsAwarded * 100 / pointsAvailable;
            return Math.Min(percentComplete, 100);
            }

        /// <summary>
        ///     Determines whether the supplied set of observations are sufficient to complete the given level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="observations">The observations.</param>
        /// <returns><c>true</c> if [is level complete] [the specified level]; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool IsLevelComplete(MissionLevel level, IEnumerable<Observation> observations)
            {
            var eligibleObservations = observations as IList<Observation> ?? observations.ToList();
            foreach (var track in level.Tracks)
                {
                var percentComplete = ComputePercentComplete(track.Challenges, eligibleObservations);
                if (percentComplete == 100) return true;
                }
            return false;
            }
        }
    }