// This file is part of the MS.Gamification project
// 
// File: RequiresAdministratorRights.cs  Created: 2016-11-01@19:37
// Last modified: 2017-05-16@20:13

using System.Web.Mvc;
using MS.Gamification.Models;

namespace MS.Gamification.Areas.Admin.Controllers
    {
    [Authorize(Roles = RoleNames.Administrator)]
    public class RequiresAdministratorRights : Controller { }
    }