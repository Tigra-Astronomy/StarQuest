// This file is part of the MS.Gamification project
// 
// File: CancelObservingSessionViewModel.cs  Created: 2017-06-19@00:38
// Last modified: 2017-06-19@00:43

namespace MS.Gamification.Areas.Admin.ViewModels.ObservingSessions
    {
    public class CancelObservingSessionViewModel
        {
        public int SessionId { get; set; }

        public bool NotifyMembers { get; set; }

        public string Message { get; set; }
        }
    }