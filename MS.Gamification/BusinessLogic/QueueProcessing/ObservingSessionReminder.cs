// This file is part of the MS.Gamification project
// 
// File: ObservingSessionReminder.cs  Created: 2017-05-18@21:37
// Last modified: 2017-05-18@22:34

using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QueueProcessing
    {
    internal class ObservingSessionReminder : QueuedWorkItem
        {
        public ObservingSessionReminder()
            {
            QueueName = "Events";
            }

        public int? ObservingSessionId { get; set; }

        public ObservingSession ObservingSession { get; set; }
        }
    }