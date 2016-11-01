// This file is part of the MS.Gamification project
// 
// File: ForgotViewModel.cs  Created: 2016-07-10@00:07
// Last modified: 2016-07-20@01:46

using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.ViewModels
    {
    public class ForgotViewModel
        {
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        }
    }
