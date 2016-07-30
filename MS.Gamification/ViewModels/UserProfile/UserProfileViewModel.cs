// This file is part of the MS.Gamification project
// 
// File: UserProfileViewModel.cs  Created: 2016-07-29@16:14
// Last modified: 2016-07-30@19:39

using System.Collections.Generic;
using MS.Gamification.ViewModels.Mission;

namespace MS.Gamification.ViewModels.UserProfile
    {
    public class UserProfileViewModel
        {
        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public IEnumerable<string> Titles { get; set; }

        public IEnumerable<string> Badges { get; set; }

        public IEnumerable<ObservationSummaryViewModel> Observations { get; set; }

        public IEnumerable<MissionProgressViewModel> Missions { get; set; }

        public string UserId { get; set; }
        }
    }