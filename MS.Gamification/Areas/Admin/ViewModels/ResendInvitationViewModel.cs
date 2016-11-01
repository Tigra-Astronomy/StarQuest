// This file is part of the MS.Gamification project
// 
// File: ResendInvitationViewModel.cs  Created: 2016-08-05@20:21
// Last modified: 2016-08-05@20:22

using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.Areas.Admin.ViewModels
    {
    public class ResendInvitationViewModel
        {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        }
    }