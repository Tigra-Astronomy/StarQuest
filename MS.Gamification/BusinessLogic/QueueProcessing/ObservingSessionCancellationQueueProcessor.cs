// This file is part of the MS.Gamification project
// 
// File: ObservingSessionCancellationQueueProcessor.cs  Created: 2017-06-19@22:39
// Last modified: 2017-06-20@13:01

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
    public class ObservingSessionCancellationQueueProcessor : IProcessWorkItems
        {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();
        private readonly IMapper mapper;
        private readonly IGameNotificationService notifier;
        private readonly IUnitOfWork uow;

        public ObservingSessionCancellationQueueProcessor([NotNull] IGameNotificationService notifier, [NotNull] IUnitOfWork uow,
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
            var cancellation = item as ObservingSessionCancellation;
            if (cancellation == null)
                {
                var message =
                    $"Expected a work item with type {nameof(ObservingSessionCancellation)} but got {item.GetType().Name}";
                log.Error(message);
                throw new ArgumentException(message, nameof(item));
                }
            var session = uow.ObservingSessions.Get(cancellation.ObservingSessionId.Value);
            var emailModel = mapper.Map<ObservingSessionCancellationEmailModel>(cancellation);
            emailModel.Session = session;
            var userSpecification = new UsersForNotifyAllSpecification();
            var usersToNotify = uow.Users.AllSatisfying(userSpecification);
            return notifier.NotifyUsersAsync(emailModel, $"Event cancelled: {session.Title}",
                usersToNotify);
            }
        }
    }