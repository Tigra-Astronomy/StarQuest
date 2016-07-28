// This file is part of the MS.Gamification project
// 
// File: LevelProgressViewModel.cs  Created: 2016-07-28@15:54
// Last modified: 2016-07-28@20:36

using System.Collections.Generic;
using MS.Gamification.GameLogic;
using MS.Gamification.Models;

namespace MS.Gamification.ViewModels.Mission
    {
    public class LevelProgressViewModel : IPreconditionXml
        {
        public int Level { get; set; }

        public bool Unlocked { get; set; }

        public List<TrackProgressViewModel> Tracks { get; set; }

        public int OverallProgressPercent { get; set; }

        public string Precondition { get; set; }
        }
    }