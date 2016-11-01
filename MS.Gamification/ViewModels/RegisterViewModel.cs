// This file is part of the MS.Gamification project
// 
// File: RegisterViewModel.cs  Created: 2016-06-05@19:58
// Last modified: 2016-06-05@20:45

using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.ViewModels
    {
    public class RegisterViewModel
        {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string UserName { get; set; }
        }
    }