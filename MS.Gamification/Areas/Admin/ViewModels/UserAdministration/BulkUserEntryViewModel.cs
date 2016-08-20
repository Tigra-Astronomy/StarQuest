// This file is part of the MS.Gamification project
// 
// File: BulkUserEntryViewModel.cs  Created: 2016-08-20@19:47
// Last modified: 2016-08-20@19:52

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MS.Gamification.Areas.Admin.ViewModels.UserAdministration
    {
    public class BulkUserEntryViewModel
        {
        [AllowHtml]
        [Display(Name = "User Details")]
        [DataType(DataType.MultilineText)]
        public string UserNamesAndEmails { get; set; }
        }
    }