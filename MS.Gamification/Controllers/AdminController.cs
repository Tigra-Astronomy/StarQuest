// This file is part of the MS.Gamification project
// 
// File: AdminController.cs  Created: 2016-03-24@00:31
// Last modified: 2016-03-24@00:35 by Fern

using System.Web.Mvc;

namespace MS.Gamification.Controllers
    {
    [Authorize(Roles = AdministratorRoleName)]
    public class AdminController : Controller
        {
        internal const string AdministratorRoleName = "Administrator";
        internal const string ModeratorRoleName = "Moderator";
        }
    }
