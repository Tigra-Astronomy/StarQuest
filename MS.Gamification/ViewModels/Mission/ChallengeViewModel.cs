// This file is part of the MS.Gamification project
// 
// File: ChallengeViewModel.cs  Created: 2016-07-28@15:58
// Last modified: 2016-07-28@16:34

namespace MS.Gamification.ViewModels.Mission
    {
    public class ChallengeViewModel
        {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Points { get; set; }

        public string Location { get; set; }

        public string BookSection { get; set; }

        public bool HasObservation { get; set; }
        }
    }