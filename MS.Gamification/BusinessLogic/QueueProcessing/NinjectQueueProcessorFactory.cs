// This file is part of the MS.Gamification project
// 
// File: NinjectQueueProcessorFactory.cs  Created: 2017-06-04@00:04
// Last modified: 2017-06-04@00:05

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using MS.Gamification.Models;
using Ninject;
using Ninject.Parameters;
using NLog;

namespace MS.Gamification.BusinessLogic.QueueProcessing
{
    internal class NinjectQueueProcessorFactory : IQueueProcessorFactory
    {
        [NotNull] private readonly ILogger Log = LogManager.GetCurrentClassLogger();
        [NotNull] private readonly IKernel services;
        private readonly IDictionary<Type, Type> workItemToQueueProcessorMapping;

        public NinjectQueueProcessorFactory([NotNull] IKernel services, IDictionary<Type, Type> workItemToQueueProcessorMapping)
        {
            Contract.Requires(services != null);
            this.services = services;
            this.workItemToQueueProcessorMapping = workItemToQueueProcessorMapping;
        }

        public IProcessWorkItems GetQueueProcessorFor(QueuedWorkItem item)
        {
            Contract.Requires(item != null);
            Contract.Ensures(Contract.Result<IProcessWorkItems>() != null);
            Log.Debug($"Creating queue processor for type {item.GetType()} using Ninject");
            var processorType = ProcessorTypeFor(item);
            var instance = (IProcessWorkItems)services.Get(processorType);
            Log.Debug($"Found queue processor type {processorType} and successfully resolved an instance");
            return instance;
        }

        [NotNull]
        private Type ProcessorTypeFor([NotNull] QueuedWorkItem item)
        {
            Contract.Requires(item != null);
            Contract.Ensures(Contract.Result<Type>() != null);
            var workItemRuntimeType = item.GetType();
            if (workItemToQueueProcessorMapping.ContainsKey(workItemRuntimeType))
            {
                return workItemToQueueProcessorMapping[workItemRuntimeType];
            }
            throw new ArgumentException($"There is no registered queue processor for work items of type {workItemRuntimeType.Name}", nameof(item));
        }

        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822: MarkMembersAsStatic", Justification = "Required for code contracts.")]
        [Conditional("CONTRACTS_FULL")]
        private void ObjectInvariant()
        {
            Contract.Invariant(Log != null);
            Contract.Invariant(services != null);
        }
    }
}