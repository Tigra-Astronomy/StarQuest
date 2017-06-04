// This file is part of the MS.Gamification project
// 
// File: IQueueProcessorFactory.cs  Created: 2017-06-03@23:14
// Last modified: 2017-06-04@00:27

using System;
using System.Diagnostics.Contracts;
using JetBrains.Annotations;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QueueProcessing
    {
    [ContractClass(typeof(QueueProcessorFactoryContract))]
    internal interface IQueueProcessorFactory
        {
        [NotNull]
        IProcessWorkItems GetQueueProcessorFor([NotNull] QueuedWorkItem item);
        }

    [ContractClassFor(typeof(IQueueProcessorFactory))]
    internal abstract class QueueProcessorFactoryContract : IQueueProcessorFactory
        {
        public IProcessWorkItems GetQueueProcessorFor(QueuedWorkItem item)
            {
            Contract.Requires(item != null);
            Contract.Ensures(Contract.Result<IProcessWorkItems>() != null);
            throw new NotImplementedException();
            }
        }
    }