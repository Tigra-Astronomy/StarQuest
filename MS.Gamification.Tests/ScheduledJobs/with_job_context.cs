// This file is part of the MS.Gamification project
// 
// File: with_job_context.cs  Created: 2016-12-13@00:08
// Last modified: 2016-12-13@01:06

using FluentScheduler;
using Machine.Specifications;
using MS.Gamification.GameLogic;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.ScheduledJobs
    {
    class with_job_context<TJob> where TJob : class, IJob
        {
        Cleanup after = () =>
            {
            Job = null;
            JobContextBuilder = null;
            };
        Establish context = () => JobContextBuilder = new ScheduledJobContextBuilder<TJob>()
            .WithData(TestData.CreateStandardMissionData);

        protected static ScheduledJobContextBuilder<TJob> JobContextBuilder { get; set; }

        protected static TJob Job { get; set; }

        public IGameNotificationService Notifier => JobContextBuilder.Notifier;
        }
    }