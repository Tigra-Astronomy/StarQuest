// This file is part of the MS.Gamification project
// 
// File: CreateUsersViewModel.cs  Created: 2016-07-17@03:20
// Last modified: 2016-07-17@03:27

using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.ViewModels
    {
    public class CreateUsersViewModel
        {
        [DataType(DataType.MultilineText)]
        [Required]
        public string EmailList { get; set; }
        }
    }
