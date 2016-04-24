// This file is part of the MS.Gamification project
// 
// File: AdminController.cs  Created: 2016-04-01@23:54
// Last modified: 2016-04-23@22:59 by Fern

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
