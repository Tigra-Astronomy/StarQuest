// This file is part of the MS.Gamification project
// 
// File: CancelObservingSessionViewModel.cs  Created: 2017-06-19@00:38
// Last modified: 2017-06-20@01:03

namespace MS.Gamification.Areas.Admin.ViewModels.ObservingSessions
    {
    public class CancelObservingSessionViewModel
        {
        /// <summary>
        /// The ObservingSession ID being cancelled. It is simply called 'Id' here to simplify model binding.
        /// </summary>
        public int Id { get; set; }

        public bool NotifyMembers { get; set; }

        public string Message { get; set; }
        }
    }