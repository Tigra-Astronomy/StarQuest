// This file is part of the MS.Gamification project
// 
// File: ObservingSessionReminderQueueProcessor.cs  Created: 2017-06-04@02:03
// Last modified: 2017-06-06@22:49

using System;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JetBrains.Annotations;
using MS.Gamification.App_Start;
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
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly IMapper mapper;
        private readonly IGameNotificationService notifier;
        private readonly IUnitOfWork uow;

        public ObservingSessionReminderQueueProcessor([NotNull] IGameNotificationService notifier, [NotNull] IUnitOfWork uow,
            [NotNull] IMapper mapper)
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
            var session = uow.ObservingSessions.Get(reminder.ObservingSessionId.Value);
            var emailModel = mapper.Map<ObservingSessionReminderEmailModel>(reminder);
            emailModel.Session = session;
            var userSpecification = new UsersForNotifyAllSpecification();
            var usersToNotify = uow.Users.AllSatisfying(userSpecification);
            return notifier.NotifyUsersAsync(emailModel, $"Upcoming event reminder: {session.Title}",
                usersToNotify);
            }
        }
    }