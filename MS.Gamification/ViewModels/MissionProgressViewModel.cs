// This file is part of the MS.Gamification project
// 
// File: MissionProgressViewModel.cs  Created: 2016-07-09@20:14
// Last modified: 2016-07-22@09:29

using System.Collections.Generic;
using MS.Gamification.Models;

namespace MS.Gamification.ViewModels
    {
    public class MissionProgressViewModel
        {
        public string MissionTitle { get; set; }

        public List<LevelProgressViewModel> Levels { get; set; } = new List<LevelProgressViewModel>();
        }

    public class LevelProgressViewModel
        {
        public int Level { get; set; }

        public bool Unlocked { get; set; }

        public IEnumerable<MissionTrack> Tracks { get; set; }

        public int OverallProgressPercent { get; set; }

        public List<int> TrackProgress { get; set; }
        }
    }