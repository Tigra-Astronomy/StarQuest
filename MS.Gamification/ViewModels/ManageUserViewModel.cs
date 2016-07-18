// This file is part of the MS.Gamification project
// 
// File: ManageUserViewModel.cs  Created: 2016-07-18@16:30
// Last modified: 2016-07-18@22:23

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MS.Gamification.ViewModels
    {
    public class ManageUserViewModel
        {
        public ManageUserViewModel()
            {
            Roles = new List<string>();
            RoleToAdd = string.Empty;
            }

        public string Id { get; set; }

        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool EmailVerified { get; set; }

        public bool HasValidPassword { get; set; }

        public bool AccountLocked { get; set; }

        public IList<string> Roles { get; set; }

        public string RoleToAdd { get; set; }
        }
    }