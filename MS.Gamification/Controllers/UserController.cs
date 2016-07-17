// This file is part of the MS.Gamification project
// 
// File: UserController.cs  Created: 2016-07-10@00:07
// Last modified: 2016-07-17@00:22

using System.Web.Mvc;

namespace MS.Gamification.Controllers
    {
    [Authorize]
    public class UserController : Controller
        {
        public ActionResult UserProfile()
            {
            return View();
            }

        public ActionResult AdminDashboard()
            {
            return View();
            }
        }
    }
