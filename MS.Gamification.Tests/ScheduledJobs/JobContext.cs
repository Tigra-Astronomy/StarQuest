// This file is part of the MS.Gamification project
// 
// File: JobContext.cs  Created: 2017-05-19@19:41
// Last modified: 2017-05-19@20:19

using System.Data.Entity;
using FluentScheduler;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.DataAccess;
using MS.Gamification.Tests.QueueProcessing;
using Ninject;

namespace MS.Gamification.Tests.ScheduledJobs
    {
    class JobContext<TJob> where TJob : class, IJob
        {
        public TJob Job { get; set; }

        public UnitTestTimeProvider TimeProvider { get; } = new UnitTestTimeProvider();

        public IUnitOfWork UnitOfWork { get; set; }

        public IGameNotificationService Notifier { get; set; }

        public DbContext DataContext { get; set; }

        public IKernel DependencyResolver { get; set; } = new StandardKernel();
        }
    }