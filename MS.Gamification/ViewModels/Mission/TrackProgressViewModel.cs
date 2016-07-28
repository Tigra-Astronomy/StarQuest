// This file is part of the MS.Gamification project
// 
// File: TrackProgressViewModel.cs  Created: 2016-07-28@15:56
// Last modified: 2016-07-28@15:57

using System.Collections.Generic;
using MS.Gamification.ViewModels.Mission;

namespace MS.Gamification.Models
    {
    public class TrackProgressViewModel
        {
        public string Name { get; set; }

        public int Number { get; set; }

        public virtual List<ChallengeViewModel> Challenges { get; set; }

        public string AwardTitle { get; set; }

        public Badge Badge { get; set; }

        public int BadgeId { get; set; }

        public int Id { get; set; }

        public int PercentComplete { get; set; }
        }
    }