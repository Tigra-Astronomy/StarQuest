// This file is part of the MS.Gamification project
// 
// File: ObservingSessionReminderQueueProcessor.cs  Created: 2017-06-04@02:03
// Last modified: 2017-06-04@02:06

using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.BusinessLogic.Gamification.QuerySpecifications;
using MS.Gamification.DataAccess;
using MS.Gamification.EmailTemplates;
using MS.Gamification.Models;
using NLog;

namespace MS.Gamification.BusinessLogic.QueueProcessing
{
    internal class ObservingSessionReminderQueueProcessor : IProcessWorkItems
    {
        private readonly IGameNotificationService notifier;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public ObservingSessionReminderQueueProcessor([NotNull] IGameNotificationService notifier, [NotNull] IUnitOfWork uow, [NotNull] IMapper mapper)
        {
            Contract.Requires(notifier != null);
            Contract.Requires(uow != null);
            Contract.Requires(mapper != null);
            this.notifier = notifier;
            this.uow = uow;
            this.mapper = mapper;
        }

        public Task ProcessWorkItemAsync(QueuedWorkItem item)
        {
            var reminder = item as ObservingSessionReminder;
            if (reminder == null)
            {
                var message = $"Expected a work item with type {nameof(ObservingSessionReminder)} but got {item.GetType().Name}";
                log.Error(message);
                throw new ArgumentException(message, nameof(item));
            }

            var session = reminder.ObservingSession;
            var userSpecification = new UsersForNotifyAllSpecification();
            var usersToNotify = uow.Users.AllSatisfying(userSpecification);
            var emailModel = mapper.Map<ObservingSessionReminderEmailModel>(reminder);
            return notifier.NotifyUsersAsync(emailModel, $"Upcoming event reminder: {reminder.ObservingSession.Title}", usersToNotify);
        }
    }
}