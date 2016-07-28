// This file is part of the MS.Gamification project
// 
// File: BadgeAwardedEmailModel.cs  Created: 2016-07-28@12:27
// Last modified: 2016-07-28@12:29

using MS.Gamification.EmailTemplates;

namespace MS.Gamification
    {
    public class BadgeAwardedEmailModel : EmailModelBase
        {
        public string BadgeName { get; set; }

        public string TrackName { get; set; }

        public string MissionTitle { get; set; }

        public string LevelAwardTitle { get; set; }
        }
    }