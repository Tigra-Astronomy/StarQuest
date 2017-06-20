// This file is part of the MS.Gamification project
// 
// File: ObservingSessionReminderEmailModel.cs  Created: 2017-06-04@02:12
// Last modified: 2017-06-19@22:45

using MS.Gamification.Models;

namespace MS.Gamification.EmailTemplates
    {
    public class ObservingSessionReminderEmailModel : EmailModelBase
        {
        public ObservingSession Session { get; set; }
        }
    }