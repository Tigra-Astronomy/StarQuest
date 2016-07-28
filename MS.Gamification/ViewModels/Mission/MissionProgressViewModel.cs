// This file is part of the MS.Gamification project
// 
// File: MissionProgressViewModel.cs  Created: 2016-07-09@20:14
// Last modified: 2016-07-22@09:29

using System.Collections.Generic;

namespace MS.Gamification.ViewModels.Mission
    {
    public class MissionProgressViewModel
        {
        public string MissionTitle { get; set; }

        public List<LevelProgressViewModel> Levels { get; set; } = new List<LevelProgressViewModel>();
        }
    }