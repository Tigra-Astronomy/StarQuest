﻿// This file is part of the MS.Gamification project
// 
// File: ModeratorDailySummaryTask.cs  Created: 2016-12-12@16:04
// Last modified: 2016-12-30@03:46

using System.Linq;
using System.Web.Hosting;
using FluentScheduler;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.Areas.Admin.Controllers;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic.QuerySpecifications;
using NLog;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;

namespace MS.Gamification.GameLogic.ScheduledTasks
    {
    [ReaderWriterSynchronized]
    public class ModeratorDailySummaryTask : IJob, IRegisteredObject
        {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        [Reference] private readonly IGameNotificationService notifier;
        [Reference] private readonly RoleManager<IdentityRole> rolemanager;
        [Reference] private readonly IUnitOfWork uow;
        private bool stopRequested; // This is the resource that is protected by [ReaderWriterSynchronized]

        public ModeratorDailySummaryTask(IGameNotificationService notifier, IUnitOfWork uow, RoleManager<IdentityRole> rolemanager)
            {
            this.notifier = notifier;
            this.uow = uow;
            this.rolemanager = rolemanager;
            HostingEnvironment.RegisterObject(this);
            }

        [Reader]
        public void Execute()
            {
            try
                {
                log.Info("Beginning scheduled operation: Moderator Daily Summary Task");
                NotifyModeratorsOfPendingRequests();
                }
            finally
                {
                log.Info("Completed scheduled operation: Moderator Daily Summary Task");
                }
            }

        [Writer]
        public void Stop(bool immediate)
            {
            stopRequested = true;
            HostingEnvironment.UnregisterObject(this);
            }

        private void NotifyModeratorsOfPendingRequests()
            {
            if (stopRequested) return;
            var observationSpecification = new ObservationsAwaitingModeration();
            var pendingObservations = uow.Observations.AllSatisfying(observationSpecification);
            log.Info($"Found {pendingObservations.Count()} pending observations");
            if (!pendingObservations.Any())
                return;
            var moderatorSpecification = new UsersInRole(RequiresAdministratorRights.ModeratorRoleName, rolemanager);
            var moderators = uow.Users.AllSatisfying(moderatorSpecification);
            log.Info($"Found {moderators.Count()} users with role {RequiresAdministratorRights.ModeratorRoleName}");
            if (!moderators.Any())
                return;
            foreach (var moderator in moderators)
                {
                notifier.PendingObservationSummary(moderator, pendingObservations);
                }
            }
        }
    }