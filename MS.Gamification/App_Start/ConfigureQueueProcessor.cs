// This file is part of the MS.Gamification project
// 
// File: ConfigureQueueProcessor.cs  Created: 2017-06-03@23:05
// Last modified: 2017-06-04@00:37

using System;
using System.Collections.Generic;
using MS.Gamification.BusinessLogic.QueueProcessing;

namespace MS.Gamification.App_Start
    {
    internal static class ConfigureQueueProcessor
        {
        internal static IDictionary<Type, Type> MapQueueProcessors() => new Dictionary<Type, Type>
            {
            [typeof(ObservingSessionReminder)] = typeof(ObservingSessionReminderQueueProcessor)
            };
        }
    }