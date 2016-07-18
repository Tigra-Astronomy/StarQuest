// This file is part of the MS.Gamification project
// 
// File: ResetPasswordViewModel.cs  Created: 2016-06-05@19:58
// Last modified: 2016-07-18@04:08

using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.ViewModels
    {
    public class ResetPasswordViewModel
        {
        [Required]
        [EmailAddress]
        [Display(Name = "Username or Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
             ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
        }
    }