// This file is part of the MS.Gamification project
// 
// File: ObservingSessionLogic.cs  Created: 2017-05-17@19:37
// Last modified: 2017-06-19@02:01

using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MS.Gamification.Areas.Admin.ViewModels.ObservingSessions;
using MS.Gamification.BusinessLogic.Gamification.QuerySpecifications;
using MS.Gamification.BusinessLogic.QueueProcessing;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using NLog;
using NLog.Fluent;

namespace MS.Gamification.BusinessLogic.EventManagement
    {
    internal class ObservingSessionLogic : IObservingSessionManager
        {
        [NotNull] private readonly ITimeProvider clock;
        [NotNull] private readonly ILogger log = LogManager.GetCurrentClassLogger();
        [NotNull] private readonly IMapper mapper;
        [NotNull] private readonly IUnitOfWork uow;

        public ObservingSessionLogic([NotNull] IMapper mapper, [NotNull] IUnitOfWork uow, [NotNull] ITimeProvider clock)
            {
            Contract.Requires(mapper != null);
            Contract.Requires(uow != null);
            Contract.Requires(clock != null);
            this.mapper = mapper;
            this.uow = uow;
            this.clock = clock;
            }

        public async Task CreateAsync([NotNull] CreateObservingSessionViewModel model)
            {
            Contract.Requires(model != null);
            log.Info()
                .Message("Creating observing session")
                .Property(nameof(model), model)
                .Write();
            var session = mapper.Map<ObservingSession>(model);
            uow.ObservingSessions.Add(session);
            await uow.CommitAsync().ConfigureAwait(false);
            log.Debug().Message("Commit successful").Property("Entity", session);
            await SetRemindersAsync(session.Id, model.SendAnnouncement).ConfigureAwait(false);
            }

        public Task DeleteAsync(int sessionId)
            {
            var maybeSession = uow.ObservingSessions.GetMaybe(sessionId);
            if (maybeSession.None)
                throw new ArgumentException("No such session ID", nameof(sessionId));
            var session = maybeSession.Single();
            RemoveRemindersForSession(session.Id);
            uow.ObservingSessions.Remove(session);
            return uow.CommitAsync();
            }

        /// <summary>
        ///     Cancels a scheduled observing session and optionally notifies users.
        /// </summary>
        /// <param name="sessionId">The identifier of the session to be cancelled.</param>
        /// <param name="notifyMembers">if set to <c>true</c> then a notification message is sent to users.</param>
        /// <param name="message">A message to users explaining the cancellation.</param>
        public async Task CancelAsync(int sessionId, bool notifyMembers, string message)
            {
            var session = GetSessionByIdOrThrow(sessionId);
            RemoveRemindersForSession(session.Id);
            session.ScheduleState = ScheduleState.Cancelled;
            await uow.CommitAsync();
            if (notifyMembers)
                await QueueCancellationNotificationAsync(session, message);
            }

        private void RemoveRemindersForSession(int sessionId)
            {
            var reminderSpecification = new RemindersForObservingSession(sessionId);
            var reminders = uow.QueuedWorkItems.AllSatisfying(reminderSpecification);
            uow.QueuedWorkItems.Remove(reminders);
            log.Debug($"Removed {reminders.Count()} reminders");
            }

        private ObservingSession GetSessionByIdOrThrow(int sessionId)
            {
            var maybeSession = uow.ObservingSessions.GetMaybe(sessionId);
            if (maybeSession.None)
                throw new ArgumentException("No such session ID", nameof(sessionId));
            var session = maybeSession.Single();
            return session;
            }

        private async Task SetRemindersAsync(int sessionId, bool immediateAnnoucement)
            {
            // It is assumed that the session is known to exist.
            var session = uow.ObservingSessions.Get(sessionId);
            var timeUntilEventStarts = session.StartsAt - clock.UtcNow;
            if (immediateAnnoucement)
                await QueueReminderAsync(session, clock.UtcNow).ConfigureAwait(false);
            if (session.RemindOneWeekBefore && timeUntilEventStarts > TimeSpan.FromDays(7))
                await QueueReminderAsync(session, session.StartsAt - TimeSpan.FromDays(7)).ConfigureAwait(false);
            if (session.RemindOneDayBefore && timeUntilEventStarts > TimeSpan.FromDays(1))
                await QueueReminderAsync(session, session.StartsAt - TimeSpan.FromDays(1)).ConfigureAwait(false);
            }

        [NotNull]
        private Task QueueReminderAsync([NotNull] ObservingSession session, DateTime when)
            {
            Contract.Requires(session != null);
            Contract.Ensures(Contract.Result<Task>() != null);
            var reminder = new ObservingSessionReminder
                {
                ObservingSessionId = session.Id,
                ProcessAfter = when,
                QueueName = "Events"
                };
            uow.QueuedWorkItems.Add(reminder);
            return uow.CommitAsync();
            }

        [NotNull]
        private Task QueueCancellationNotificationAsync([NotNull] ObservingSession session, string message)
            {
            Contract.Requires(session != null);
            Contract.Ensures(Contract.Result<Task>() != null);
            var cancellation = new ObservingSessionCancellation
                {
                ObservingSessionId = session.Id,
                ProcessAfter = DateTime.UtcNow,
                QueueName = "Events",
                Message = message
                };
            uow.QueuedWorkItems.Add(cancellation);
            return uow.CommitAsync();
            }
        }
    }