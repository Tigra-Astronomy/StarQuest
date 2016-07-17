// This file is part of the MS.Gamification project
// 
// File: RequiresAdministratorRights.cs  Created: 2016-06-30@21:44
// Last modified: 2016-07-17@07:48

using System.Web.Mvc;

namespace MS.Gamification.Controllers
    {
    [Authorize(Roles = AdministratorRoleName)]
    public class RequiresAdministratorRights : Controller
        {
        internal const string AdministratorRoleName = "Administrator";
        internal const string ModeratorRoleName = "Moderator";
        }
    }