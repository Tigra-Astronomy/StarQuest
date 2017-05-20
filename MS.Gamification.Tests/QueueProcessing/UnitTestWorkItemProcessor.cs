// This file is part of the MS.Gamification project
// 
// File: UnitTestWorkItemProcessor.cs  Created: 2017-05-19@22:05
// Last modified: 2017-05-20@01:17

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MS.Gamification.BusinessLogic.QueueProcessing;
using MS.Gamification.Models;
using NLog;
using NLog.Fluent;

namespace MS.Gamification.Tests.QueueProcessing
    {
    class UnitTestWorkItemProcessor : IProcessWorkItems
        {
        readonly ILogger log = LogManager.GetCurrentClassLogger();
        readonly List<UnitTestWorkItem> processedItems = new List<UnitTestWorkItem>();
        readonly List<QueuedWorkItem> skippedItems = new List<QueuedWorkItem>();

        public int ProcessedItemsCount => processedItems.Count;

        public int SkippedItemsCount => skippedItems.Count;

        public Task ProcessWorkItemAsync(QueuedWorkItem item)
            {
            var workItem = item as UnitTestWorkItem;
            if (workItem != null) return ProcessWorkItemAsync(workItem);
            skippedItems.Add(item);
            var message = $"Expected {typeof(UnitTestWorkItem).Name} but got {item.GetType().Name}";
            log.Error().Message(message).Property("item", item).Write();
            throw new ArgumentException(message);
            }

        [NotNull]
        Task ProcessWorkItemAsync([NotNull] UnitTestWorkItem item)
            {
            Contract.Requires(item != null);
            Contract.Ensures(Contract.Result<Task>() != null);
            processedItems.Add(item);
            return Task.FromResult(0);
            }
        }
    }