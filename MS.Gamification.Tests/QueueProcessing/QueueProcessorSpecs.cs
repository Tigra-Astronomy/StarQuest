// This file is part of the MS.Gamification project
// 
// File: QueueProcessorSpecs.cs  Created: 2017-05-19@18:54
// Last modified: 2017-05-20@01:47

using System;
using System.Linq;
using Machine.Specifications;
using MS.Gamification.BusinessLogic.QueueProcessing;
using MS.Gamification.Tests.QueueProcessing;

namespace Namespace
    {
    /*
     * Queue Processor:
     * When the queue is empty
     *  + Calling the processor should not process anything
     * When the queue has one work item of type QueuedWorkItem
     *  + Calling the processor should not process anything
     * When the queue has 1 eligible item and there is a matching queueu processor
     *  + It should process the item
     *  + The item should be marked as completed
     */
    [Subject(typeof(QueueProcessorTask))]
    class when_the_processor_finds_an_empty_queue : with_queue_context
        {
        Establish context = () => Context = JobContextBuilder.Build();
        Because of = () => Job.Execute();
        //It should_not_change_the_database = () => DbContext.ChangeTracker.HasChanges().ShouldBeFalse();
        It should_not_process_any_work_items = () => WorkItemProcessor.ProcessedItemsCount.ShouldEqual(0);
        It should_not_find_any_eligible_work_items = () => Job.EligibleWorkItems.Count.ShouldEqual(0);
        }

    [Subject(typeof(QueueProcessorTask), "no queue processor for type")]
    class when_the_queue_has_one_item_with_no_queue_processor_available : with_queue_context
        {
        Establish context = () => Context = JobContextBuilder
            .WithQueuedWorkItem(new ObservingSessionReminder
                {
                Id = 1, Disposition = WorkItemDisposition.Pending,
                ProcessAfter = Y2K
                })
            .StartTime(Y2K)
            .Build();
        Because of = () => Job.Execute();
        //It should_not_change_the_database = () => DbContext.ChangeTracker.HasChanges().ShouldBeFalse();
        It should_not_process_any_work_items = () => WorkItemProcessor.ProcessedItemsCount.ShouldEqual(0);
        It should_find_the_eligible_work_items = () => Job.EligibleWorkItems.Count.ShouldEqual(1);
        static readonly DateTime Y2K = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

    /*
     * When the queue has 1 eligible item and there is a matching queueu processor
     *  + It should process the item
     *  + The item should be marked as completed
     */
    [Subject(typeof(QueueProcessorTask), "eligible work item")]
    class when_the_queue_has_one_eligible_item_and_a_matching_queue_processor : with_queue_context
        {
        Establish context = () => Context = JobContextBuilder
            .WithQueuedWorkItem(new UnitTestWorkItem
                {
                Id = 1, Disposition = WorkItemDisposition.Pending, ProcessAfter = startTime
                })
            .Build();
        Because of = () => Job.Execute();
        It should_discover_1_eligible_item = () => Job.EligibleWorkItems.Count.ShouldEqual(1);
        It should_process_the_item = () => Job.SuccessfulWorkItems.Count.ShouldEqual(1);
        It should_not_fail_any_items = () => Job.FailedWorkItems.Count.ShouldEqual(0);
        It should_not_skip_any_items = () => Job.SkippedWorkItems.Count.ShouldEqual(0);
        It should_mark_the_item_as_completed = () => Job.SuccessfulWorkItems.Single().Disposition
            .ShouldEqual(WorkItemDisposition.Completed);
        static readonly DateTime startTime = DateTime.UtcNow;
        }
    }