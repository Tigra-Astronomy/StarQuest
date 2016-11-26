// This file is part of the MS.Gamification project
// 
// File: ManageUserViewModel.cs  Created: 2016-11-01@19:37
// Last modified: 2016-11-25@22:31

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MS.Gamification.Areas.Admin.ViewModels
    {
    public class ManageUserViewModel
        {
        public ManageUserViewModel()
            {
            Roles = new List<string>();
            RoleToAdd = string.Empty;
            }

        public string Id { get; set; }

        public bool Selected { get; set; }

        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool EmailVerified { get; set; }

        public bool HasValidPassword { get; set; }

        public bool AccountLocked { get; set; }

        public IList<string> Roles { get; set; }

        public string RoleToAdd { get; set; }

        public IEnumerable<SelectListItem> RolePicker { get; set; }
        }
    }