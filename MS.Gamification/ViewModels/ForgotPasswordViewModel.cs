// This file is part of the MS.Gamification project
// 
// File: AccountViewModels.cs  Created: 2016-03-23@23:23
// Last modified: 2016-03-24@00:18 by Fern

using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.ViewModels
    {
    public class ForgotPasswordViewModel
        {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        }
    }
