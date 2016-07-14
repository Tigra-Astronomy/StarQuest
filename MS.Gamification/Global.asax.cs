// This file is part of the MS.Gamification project
// 
// File: Global.asax.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-13@23:33

using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MS.Gamification
    {
    public class MvcApplication : HttpApplication
        {
        protected void Application_Start()
            {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            }
        }
    }