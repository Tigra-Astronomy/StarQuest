// This file is part of the MS.Gamification project
// 
// File: with_job_context.cs  Created: 2017-05-16@17:41
// Last modified: 2017-05-19@20:06

using System.Data.Entity;
using FluentScheduler;
using JetBrains.Annotations;
using Machine.Specifications;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.ScheduledJobs
    {
    class with_job_context<TJob> where TJob : class, IJob
        {
        Cleanup after = () =>
            {
            JobContextBuilder = null;
            Context = null;
            };
        Establish context = () => JobContextBuilder = new ScheduledJobContextBuilder<TJob>()
            .WithData(TestData.CreateStandardMissionData);

        protected static ScheduledJobContextBuilder<TJob> JobContextBuilder { get; private set; }

        protected static TJob Job => Context.Job;

        protected static JobContext<TJob> Context { get; set; }

        [NotNull]
        protected static IGameNotificationService Notifier => Context.Notifier;

        [NotNull]
        protected static DbContext DbContext => Context.DataContext;
        }
    }