// This file is part of the MS.Gamification project
// 
// File: VerificationTokenEmailModel.cs  Created: 2016-07-18@16:18
// Last modified: 2016-07-22@14:37

using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.EmailTemplates
    {
    public class VerificationTokenEmailModel
        {
        public string VerificationToken { get; set; }

        public string ApplicationName { get; set; }


        public string InformationUrl { get; set; }

        public string CallbackUrl { get; set; }

        [EmailAddress]
        public string Recipient { get; set; }
        }
    }