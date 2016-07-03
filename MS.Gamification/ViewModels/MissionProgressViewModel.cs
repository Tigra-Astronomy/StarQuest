// This file is part of the MS.Gamification project
// 
// File: MissionProgressViewModel.cs  Created: 2016-07-01@19:48
// Last modified: 2016-07-03@00:58

using System.Collections.Generic;
using MS.Gamification.Models;

namespace MS.Gamification.ViewModels
    {
    public class MissionProgressViewModel
        {
        public int Level { get; set; }

        public IEnumerable<MissionTrack> Tracks { get; set; }

        public int OverallProgressPercent { get; set; }
        }
    }