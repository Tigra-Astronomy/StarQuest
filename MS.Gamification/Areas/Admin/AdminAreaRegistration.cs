// This file is part of the MS.Gamification project
// 
// File: AdminAreaRegistration.cs  Created: 2016-08-05@19:30
// Last modified: 2016-08-06@11:27

using System.Web.Mvc;

namespace MS.Gamification.Areas.Admin
    {
    public class AdminAreaRegistration : AreaRegistration
        {
        public override string AreaName
            {
            get { return "Admin"; }
            }

        public override void RegisterArea(AreaRegistrationContext context)
            {
            context.MapRoute(
                "Admin",
                "Admin/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
                );
            }
        }
    }