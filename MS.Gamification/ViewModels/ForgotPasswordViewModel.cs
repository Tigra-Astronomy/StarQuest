// This file is part of the MS.Gamification project
// 
// File: ForgotPasswordViewModel.cs  Created: 2016-06-05@19:58
// Last modified: 2016-07-18@04:05

using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.ViewModels
    {
    public class ForgotPasswordViewModel
        {
        [Required]
        [EmailAddress]
        [Display(Name = "Username or Email")]
        public string Email { get; set; }
        }
    }