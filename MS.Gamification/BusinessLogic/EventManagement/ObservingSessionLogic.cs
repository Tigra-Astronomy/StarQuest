// This file is part of the MS.Gamification project
// 
// File: ObservingSessionLogic.cs  Created: 2017-05-17@19:37
// Last modified: 2017-05-31@13:01

using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MS.Gamification.Areas.Admin.ViewModels.ObservingSessions;
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
            log.Info().Message("Creating observing session")
                .Property(nameof(model), model)
                .Write();
            var session = mapper.Map<ObservingSession>(model);
            uow.ObservingSessions.Add(session);
            await uow.CommitAsync().ConfigureAwait(false);
            log.Debug().Message("Commit successful").Property("Entity", session);
            await SetRemindersAsync(session.Id, model.SendAnnouncement).ConfigureAwait(false);
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
        }
    }