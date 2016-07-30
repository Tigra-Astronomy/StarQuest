// This file is part of the MS.Gamification project
// 
// File: IGameEngineService.cs  Created: 2016-07-26@07:01
// Last modified: 2016-07-30@13:47

using System.Collections.Generic;
using System.Threading.Tasks;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic
    {
    public interface IGameEngineService
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
        int ComputePercentComplete(IEnumerable<Challenge> challenges, IEnumerable<Observation> eligibleObservations);

        /// <summary>
        ///     Creates observations in bulk, for the specified list of users.
        /// </summary>
        /// <param name="observation">The observation template.</param>
        /// <param name="userIds">A list of user IDs.</param>
        BatchCreateObservationsResult BatchCreateObservations(Observation observation, IEnumerable<string> userIds);

        /// <summary>
        ///     Evaluates whether the user is entitled to any new badges, as a result of submitting an observation.
        /// </summary>
        /// <param name="observation">The observation that has just been approved for the user.</param>
        Task EvaluateBadges(Observation observation);

        /// <summary>
        ///     Determines whether a level is unlocked for a user by evaluating the level preconditions against that user.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if [is level unlocked for user] [the specified level]; otherwise, <c>false</c>.</returns>
        bool IsLevelUnlockedForUser(IPreconditionXml level, string userId);

        /// <summary>
        ///     Determines whether the supplied set of observations are sufficient to complete the given level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="observations">The observations.</param>
        /// <returns><c>true</c> if [is level complete] [the specified level]; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        bool IsLevelComplete(MissionLevel level, IEnumerable<Observation> observations);
        }
    }