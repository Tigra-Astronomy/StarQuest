// This file is part of the MS.Gamification project
// 
// File: ModeratorDailySummaryTask.cs  Created: 2016-12-12@16:04
// Last modified: 2016-12-13@01:09

using System.Web.Hosting;
using FluentScheduler;
using MS.Gamification.DataAccess;
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
        [Reference] private readonly IUnitOfWork uow;
        private bool stopRequested; // This is the resource that is protected by [ReaderWriterSynchronized]

        public ModeratorDailySummaryTask(IGameNotificationService notifier, IUnitOfWork uow)
            {
            this.notifier = notifier;
            this.uow = uow;
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
            }
        }
    }