// This file is part of the MS.Gamification project
// 
// File: UserController.cs  Created: 2016-07-10@00:07
// Last modified: 2016-07-17@03:55

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

        public ActionResult CreateUserAccounts()
            {
            return View();
            }

        [HttpPost]
        public ActionResult CreateUserAccounts(string emails)
            {
            return View(nameof(CreateUserAccountsConfirmation));
            }

        public ActionResult CreateUserAccountsConfirmation()
            {
            return View();
            }
        }
    }
