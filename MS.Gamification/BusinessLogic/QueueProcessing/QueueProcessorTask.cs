// This file is part of the MS.Gamification project
// 
// File: QueueProcessorTask.cs  Created: 2017-05-19@02:05
// Last modified: 2017-05-20@01:41

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using FluentScheduler;
using JetBrains.Annotations;
using Microsoft.Ajax.Utilities;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using NLog;
using NLog.Fluent;

namespace MS.Gamification.BusinessLogic.QueueProcessing
{
    internal class QueueProcessorTask : IJob
    {
        [NotNull] private readonly ITimeProvider clock;
        [NotNull] private readonly ILogger log = LogManager.GetCurrentClassLogger();
        [NotNull] private readonly IUnitOfWork uow;
        [NotNull] private readonly IDictionary<Type, IProcessWorkItems> workItemProcessors;
        [NotNull] private readonly IQueueProcessorFactory factory;

        public QueueProcessorTask([NotNull] ITimeProvider clock, [NotNull] IUnitOfWork uow, [NotNull] IQueueProcessorFactory factory)
        {
            Contract.Requires(clock != null);
            Contract.Requires(uow != null);
            Contract.Requires(workItemProcessors != null);
            Contract.Requires(factory != null);
            this.clock = clock;
            this.uow = uow;
            this.factory = factory;
        }

        /// <summary>
        ///     The list of all work items founf to be eligible for processing
        /// </summary>
        public List<QueuedWorkItem> EligibleWorkItems { get; } = new List<QueuedWorkItem>();

        /// <summary>
        ///     The list of work items that were eligible and completed successfully.
        /// </summary>
        public List<QueuedWorkItem> SuccessfulWorkItems { get; } = new List<QueuedWorkItem>();

        /// <summary>
        ///     The list of work items that were eligible, but produced an error when run
        /// </summary>
        public List<QueuedWorkItem> FailedWorkItems { get; } = new List<QueuedWorkItem>();

        /// <summary>
        ///     The list of items that were eligible, but had not suitable queue processor.
        /// </summary>
        public List<QueuedWorkItem> SkippedWorkItems { get; } = new List<QueuedWorkItem>();

        public void Execute()
        {
            ExecuteAsync().Wait();
        }

        private async Task ExecuteAsync()
        {
            var stamp = clock.UtcNow;
            log.Info().Message($"Begin queue processing at {stamp}").Property("timestamp", stamp).Write();
            var specification = new EligibleWorkItemsSpecification(stamp);
            var eligibleWorkItems = uow.QueuedWorkItems.AllSatisfying(specification);
            EligibleWorkItems.AddRange(eligibleWorkItems);
            eligibleWorkItems.ForEach(item => item.Disposition = WorkItemDisposition.InProgress);
            await uow.CommitAsync().ConfigureAwait(false);
            await DispatchWorkItemsAsync(eligibleWorkItems).ConfigureAwait(false);
            log.Info().Message($"End queue processing begun at {stamp}").Property("timestamp", stamp).Write();
        }

        private async Task DispatchWorkItemsAsync([ItemNotNull] [NotNull] IEnumerable<QueuedWorkItem> eligibleWorkItems)
        {
            Contract.Requires(eligibleWorkItems != null);
            var itemsCount = eligibleWorkItems.Count();
            log.Debug().Message($"Dispatching {itemsCount} work items").Property("items", eligibleWorkItems).Write();
            foreach (var workItem in eligibleWorkItems)
            {
                var runtimeType = workItem.GetType();
                log.Debug()
                    .Message(
                        $"Dispatching item type {runtimeType.Name} scheduled at {workItem.ProcessAfter} in queue {workItem.QueueName}")
                    .Property("item", workItem)
                    .Write();
            IProcessWorkItems workItemProcessor;
            try
                {
                workItemProcessor = factory.GetQueueProcessorFor(workItem);
                }
            catch (Exception e)
                {
                log.Warn()
                    .Message($"No processor found for work item type {runtimeType.Name}")
                    .Property("item", workItem)
                    .Write();
                await MarkWorkItemNotRunAsync(workItem).ConfigureAwait(false);
                continue;
                }
            try
                {
                    await workItemProcessor.ProcessWorkItemAsync(workItem).ConfigureAwait(false);
                    await MarkWorkItemCompletedAsync(workItem).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await MarkWorkItemFailedAsync(workItem, ex).ConfigureAwait(false);
                }
            }
            log.Debug().Message($"Completed dispatch of {itemsCount} work items").Property("items", eligibleWorkItems).Write();
        }

        [NotNull]
        private Task MarkWorkItemCompletedAsync([NotNull] QueuedWorkItem workItem)
        {
            Contract.Requires(workItem != null);
            Contract.Ensures(Contract.Result<Task>() != null);
            SuccessfulWorkItems.Add(workItem);
            return SetWorkItemDispositionAsync(workItem, WorkItemDisposition.Completed);
        }

        private async Task MarkWorkItemFailedAsync([NotNull] QueuedWorkItem workItem, [NotNull] Exception exception)
        {
            Contract.Requires(workItem != null);
            Contract.Requires(exception != null);
            log.Error().Message($"Work item failed with error: {exception.Message}")
                .Property("item", workItem)
                .Property("exception", exception)
                .Write();
            FailedWorkItems.Add(workItem);
            await SetWorkItemDispositionAsync(workItem, WorkItemDisposition.Failed).ConfigureAwait(false);
        }

        [NotNull]
        private Task MarkWorkItemNotRunAsync([NotNull] QueuedWorkItem workItem)
        {
            SkippedWorkItems.Add(workItem);
            return SetWorkItemDispositionAsync(workItem, WorkItemDisposition.NotRun);
        }

        private async Task SetWorkItemDispositionAsync([NotNull] QueuedWorkItem item, WorkItemDisposition disposition)
        {
            Contract.Requires(item != null);
            item.Disposition = disposition;
            await uow.CommitAsync().ConfigureAwait(false);
            log.Debug().Message($"Work item disposition set to {disposition}")
                .Property("item", item)
                .Property("disposition", disposition)
                .Write();
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822: MarkMembersAsStatic", Justification = "Required for code contracts.")]
        [Conditional("CONTRACTS_FULL")]
        private void ObjectInvariant()
        {
            Contract.Invariant(workItemProcessors != null);
            Contract.Invariant(clock != null);
            Contract.Invariant(uow != null);
            Contract.Invariant(log != null);
        }
    }

    public class EligibleWorkItemsSpecification : QuerySpecification<QueuedWorkItem>
    {
        private readonly DateTime stamp;

        public EligibleWorkItemsSpecification(DateTime stamp)
        {
            this.stamp = stamp;
        }

        [NotNull]
        [ItemNotNull]
        public override IQueryable<QueuedWorkItem> GetQuery([ItemNotNull] [NotNull] IQueryable<QueuedWorkItem> items)
        {
            Contract.Requires(items != null);
            Contract.Ensures(Contract.Result<IQueryable<QueuedWorkItem>>() != null);
            var query = from workItem in items
                        where workItem.Disposition == WorkItemDisposition.Pending
                        where workItem.ProcessAfter <= stamp
                        // ToDo: add clause to check if the queue is enabled or paused
                        select workItem;
            return query;
        }
    }
}