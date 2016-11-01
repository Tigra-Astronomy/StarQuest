// This file is part of the MS.Gamification project
// 
// File: MissionProgressViewModel.cs  Created: 2016-07-28@15:54
// Last modified: 2016-08-06@13:22

using System.Collections.Generic;

namespace MS.Gamification.ViewModels.Mission
    {
    public class MissionProgressViewModel
        {
        public string MissionTitle { get; set; }

        public int Id { get; set; }

        public List<LevelProgressViewModel> Levels { get; set; } = new List<LevelProgressViewModel>();
        }
    }