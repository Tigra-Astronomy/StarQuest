// This file is part of the MS.Gamification project
// 
// File: ObservingSessionReminderQueueProcessor.cs  Created: 2017-06-04@00:36
// Last modified: 2017-06-04@00:36

using System.Threading.Tasks;
using MS.Gamification.BusinessLogic.QueueProcessing;
using MS.Gamification.Models;

namespace MS.Gamification
    {
    internal class ObservingSessionReminderQueueProcessor : IProcessWorkItems
        {
        public Task ProcessWorkItemAsync(QueuedWorkItem item)
            {
            return Task.FromResult(0);
            }
        }
    }