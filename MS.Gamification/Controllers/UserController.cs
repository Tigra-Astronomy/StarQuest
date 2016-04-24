// This file is part of the MS.Gamification project
// 
// File: UserController.cs  Created: 2016-04-23@22:59
// Last modified: 2016-04-23@23:01 by Fern

using System.Web.Mvc;

namespace MS.Gamification.Controllers
    {
    [Authorize]
    public abstract class UserController : Controller {}
    }
