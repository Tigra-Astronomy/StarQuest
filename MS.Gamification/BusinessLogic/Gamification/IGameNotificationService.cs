// This file is part of the MS.Gamification project
// 
// File: IGameNotificationService.cs  Created: 2017-05-17@19:32
// Last modified: 2017-06-04@03:03

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MS.Gamification.EmailTemplates;
using MS.Gamification.Models;
using MS.Gamification.ViewModels.Moderation;

namespace MS.Gamification.BusinessLogic.Gamification
    {
    /// <summary>
    ///     A service that can notify users about certain game-related events.
    ///     Note that the notification service is only responsible for rendering the requested notification
    ///     on demand and it will not carry out any validation of game logic.
    /// </summary>
    [ContractClass(typeof(GameNotificationServiceContract))]
    public interface IGameNotificationService
        {
        /// <summary>
        ///     Notifies the user that an observation they submitted has been approved by a moderator.
        /// </summary>
        /// <param name="observation">The observation that has been approved.</param>
        /// <returns>An awaitable Task.</returns>
        [NotNull]
        Task ObservationApprovedAsync([NotNull] Observation observation);

        [NotNull]
        Task BadgeAwardedAsync([NotNull] Badge badge, [NotNull] ApplicationUser user, [NotNull] MissionTrack track);

        /// <summary>
        ///     Notifies a user about pending observations.
        /// </summary>
        /// <param name="user">The user to be notified.</param>
        /// <param name="observations">The list of pending observations.</param>
        /// <returns>Task.</returns>
        [NotNull]
        Task PendingObservationSummaryAsync([NotNull] ApplicationUser user,
            [ItemNotNull] [NotNull] IEnumerable<ModerationQueueItem> observations);

        [NotNull]
        Task NotifyUsersAsync<TModel>([NotNull] TModel model, [NotNull] string subject,
            [ItemNotNull] [NotNull] IEnumerable<string> userIds) where TModel : EmailModelBase;
        }

    [ContractClassFor(typeof(IGameNotificationService))]
    internal abstract class GameNotificationServiceContract : IGameNotificationService
        {
        /// <summary>
        ///     Notifies the user that an observation they submitted has been approved by a moderator.
        /// </summary>
        /// <param name="observation">The observation that has been approved.</param>
        /// <returns>An awaitable Task.</returns>
        public Task ObservationApprovedAsync(Observation observation)
            {
            Contract.Requires(observation != null);
            Contract.Ensures(Contract.Result<Task>() != null);
            throw new NotImplementedException();
            }

        public Task BadgeAwardedAsync(Badge badge, ApplicationUser user, MissionTrack track)
            {
            Contract.Requires(badge != null);
            Contract.Requires(user != null);
            Contract.Requires(track != null);
            Contract.Ensures(Contract.Result<Task>() != null);
            throw new NotImplementedException();
            }


        /// <summary>
        ///     Notifies a user about pending observations.
        /// </summary>
        /// <param name="user">The user to be notified.</param>
        /// <param name="observations">The list of pending observations.</param>
        /// <returns>Task.</returns>
        public Task PendingObservationSummaryAsync(ApplicationUser user, IEnumerable<ModerationQueueItem> observations)
            {
            Contract.Requires(user != null);
            Contract.Requires(observations != null);
            Contract.Ensures(Contract.Result<Task>() != null);
            throw new NotImplementedException();
            }

        public Task NotifyUsersAsync<TModel>(TModel model, string subject, IEnumerable<string> userIds)
            where TModel : EmailModelBase
            {
            Contract.Requires(model != null);
            Contract.Requires(subject != null);
            Contract.Requires(userIds != null);
            Contract.Ensures(Contract.Result<Task>() != null);
            throw new NotImplementedException();
            }
        }
    }