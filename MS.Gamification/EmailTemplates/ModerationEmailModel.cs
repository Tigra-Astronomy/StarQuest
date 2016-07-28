// This file is part of the MS.Gamification project
// 
// File: ModerationEmailModel.cs  Created: 2016-07-27@20:46
// Last modified: 2016-07-27@21:56

namespace MS.Gamification.EmailTemplates
    {
    public class ModerationEmailModel : EmailModelBase
        {
        public string ChallengeName { get; set; }

        public int Points { get; set; }
        }
    }