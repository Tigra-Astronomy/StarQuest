// This file is part of the MS.Gamification project
// 
// File: IProcessWorkItems.cs  Created: 2017-05-18@21:38
// Last modified: 2017-05-19@21:27

using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QueueProcessing
    {
    [ContractClass(typeof(ProcessWorkItemsContract))]
    internal interface IProcessWorkItems
        {
        [NotNull]
        Task ProcessWorkItemAsync([NotNull] QueuedWorkItem item);
        }

    [ContractClassFor(typeof(IProcessWorkItems))]
    internal abstract class ProcessWorkItemsContract : IProcessWorkItems
        {
        public Task ProcessWorkItemAsync(QueuedWorkItem item)
            {
            Contract.Requires(item != null);
            Contract.Ensures(Contract.Result<Task>() != null);
            throw new NotImplementedException();
            }
        }
    }