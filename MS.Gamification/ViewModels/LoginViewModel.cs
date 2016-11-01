// This file is part of the MS.Gamification project
// 
// File: LoginViewModel.cs  Created: 2016-06-05@19:58
// Last modified: 2016-07-18@04:06

using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.ViewModels
    {
    public class LoginViewModel
        {
        [Required]
        [Display(Name = "Username or Email")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        }
    }