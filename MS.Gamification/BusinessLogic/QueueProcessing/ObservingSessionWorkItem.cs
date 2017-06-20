// This file is part of the MS.Gamification project
// 
// File: ObservingSessionWorkItem.cs  Created: 2017-06-20@00:26
// Last modified: 2017-06-20@00:27

using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QueueProcessing
    {
    public abstract class ObservingSessionWorkItem : QueuedWorkItem
        {
        public ObservingSessionWorkItem()
            {
            QueueName = "Events";
            }

        public int? ObservingSessionId { get; set; }
        }
    }