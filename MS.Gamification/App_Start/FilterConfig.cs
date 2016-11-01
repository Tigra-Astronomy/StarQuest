// This file is part of the MS.Gamification project
// 
// File: FilterConfig.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-29@12:10

using System.Web.Mvc;
using MS.Gamification.Diagnostics;

namespace MS.Gamification
    {
    public class FilterConfig
        {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
            {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ApplicationInsightsHandleErrorAttribute());
            }
        }
    }