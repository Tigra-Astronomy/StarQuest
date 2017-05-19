// This file is part of the MS.Gamification project
// 
// File: IProcessWorkItems.cs  Created: 2017-05-18@21:38
// Last modified: 2017-05-18@22:32

using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QueueProcessing
    {
    [ContractClass(typeof(ProcessWorkItemsContract<>))]
    internal interface IProcessWorkItems<in TWorkItem>
        where TWorkItem : QueuedWorkItem
        {
        [NotNull]
        Task ProcessWorkItemAsync([NotNull] TWorkItem item);
        }

    [ContractClassFor(typeof(IProcessWorkItems<>))]
    internal abstract class ProcessWorkItemsContract<TWorkItem> : IProcessWorkItems<TWorkItem> where TWorkItem : QueuedWorkItem
        {
        public Task ProcessWorkItemAsync(TWorkItem item)
            {
            Contract.Requires(item != null);
            Contract.Ensures(Contract.Result<TWorkItem>() != null);
            throw new NotImplementedException();
            }
        }
    }