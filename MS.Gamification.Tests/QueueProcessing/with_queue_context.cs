// This file is part of the MS.Gamification project
// 
// File: with_queue_context.cs  Created: 2017-05-19@18:39
// Last modified: 2017-05-19@22:40

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using Machine.Specifications;
using MS.Gamification.BusinessLogic.QueueProcessing;
using MS.Gamification.Models;
using MS.Gamification.Tests.ScheduledJobs;

namespace MS.Gamification.Tests.QueueProcessing
    {
    class with_queue_context : with_job_context<QueueProcessorTask>
        {
        [NotNull] protected static UnitTestWorkItemProcessor WorkItemProcessor = new UnitTestWorkItemProcessor();
        Establish context = () =>
            {
            var processors = new Dictionary<Type, IProcessWorkItems>
                {
                [typeof(UnitTestWorkItem)] = WorkItemProcessor
                };
            JobContextBuilder.AddDependency(kernel =>
                kernel.Bind<IDictionary<Type, IProcessWorkItems>>()
                    .ToMethod(m => processors)
                    .InTransientScope());
            };
        }

    internal static class JobContextBuilderExtensions
        {
        public static ScheduledJobContextBuilder<QueueProcessorTask> WithQueuedWorkItem(this ScheduledJobContextBuilder<QueueProcessorTask> builder, QueuedWorkItem item)
            {
            Contract.Ensures(Contract.Result<ScheduledJobContextBuilder<QueueProcessorTask>>() != null);
            return builder.WithData(d => d.WithQueuedWorkItem(item));
            }

    }
}