// This file is part of the MS.Gamification project
// 
// File: EmailModelBase.cs  Created: 2016-07-27@20:47
// Last modified: 2016-07-27@21:56

using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.EmailTemplates
    {
    public abstract class EmailModelBase
        {
        public string ApplicationName => "Star Quest";

        public string InformationUrl { get; set; }

        [EmailAddress]
        public string Recipient { get; set; }
        }
    }