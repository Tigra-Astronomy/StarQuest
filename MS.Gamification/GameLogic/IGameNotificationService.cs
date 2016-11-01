// This file is part of the MS.Gamification project
// 
// File: IGameNotificationService.cs  Created: 2016-07-27@21:10
// Last modified: 2016-07-27@21:43

using System.Threading.Tasks;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic
    {
    /// <summary>
    /// A service that can notify users about certain game-related events.
    /// Note that the notification service is only responsible for rendering the requested notification
    /// on demand and it will not carry out any validation of game logic.
    /// </summary>
    public interface IGameNotificationService
        {
        /// <summary>
        ///     Notifies the user that an observation they submitted has been approved by a moderator.
        /// </summary>
        /// <param name="observation">The observation that has been approved.</param>
        /// <returns>An awaitable Task.</returns>
        Task ObservationApproved(Observation observation);

        Task BadgeAwarded(Badge badge, ApplicationUser user, MissionTrack track);
        }
    }