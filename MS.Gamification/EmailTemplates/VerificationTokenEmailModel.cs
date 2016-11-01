// This file is part of the MS.Gamification project
// 
// File: VerificationTokenEmailModel.cs  Created: 2016-07-18@16:18
// Last modified: 2016-07-27@20:47

namespace MS.Gamification.EmailTemplates
    {
    public class VerificationTokenEmailModel : EmailModelBase
        {
        public string VerificationToken { get; set; }


        public string CallbackUrl { get; set; }
        }
    }