// This file is part of the MS.Gamification project
// 
// File: LevelProgressViewModel.cs  Created: 2016-07-28@15:54
// Last modified: 2016-07-29@20:34

using System.Collections.Generic;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.Models;

namespace MS.Gamification.ViewModels.Mission
    {
    public class LevelProgressViewModel : IPreconditionXml
        {
        public int Level { get; set; }

        public bool Unlocked { get; set; }

        public List<TrackProgressViewModel> Tracks { get; set; }

        public int OverallProgressPercent { get; set; }

        public int Id { get; set; }

        public string Precondition { get; set; }
        }
    }