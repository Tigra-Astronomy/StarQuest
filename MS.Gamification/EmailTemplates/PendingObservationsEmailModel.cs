// This file is part of the MS.Gamification project
// 
// File: PendingObservationsEmailModel.cs  Created: 2016-12-31@13:07
// Last modified: 2016-12-31@13:25

using System.Collections.Generic;
using MS.Gamification.ViewModels.Moderation;

namespace MS.Gamification.EmailTemplates
    {
    public class PendingObservationsEmailModel : EmailModelBase
        {
        public IEnumerable<ModerationQueueItem> PendingObservations { get; set; }
        }
    }