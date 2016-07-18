// This file is part of the MS.Gamification project
// 
// File: VerificationTokenEmailModel.cs  Created: 2016-07-17@06:58
// Last modified: 2016-07-17@08:24

namespace MS.Gamification.EmailTemplates
    {
    public class VerificationTokenEmailModel
        {
        public string VerificationToken { get; set; }

        public string ApplicationName { get; set; }


        public string InformationUrl { get; set; }

        public string CallbackUrl { get; set; }
        }
    }