// This file is part of the MS.Gamification project
// 
// File: ApplicationInsightsHandleErrorAttribute.cs  Created: 2016-07-29@12:06
// Last modified: 2016-07-29@12:10

using System;
using System.Web.Mvc;
using Microsoft.ApplicationInsights;

namespace MS.Gamification.Diagnostics
    {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ApplicationInsightsHandleErrorAttribute : HandleErrorAttribute
        {
        public override void OnException(ExceptionContext filterContext)
            {
            if (filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null)
                {
                //If customError is Off, then AI HTTPModule will report the exception
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                    {
                    // Note: A single instance of telemetry client is sufficient to track multiple telemetry items.
                    var ai = new TelemetryClient();
                    ai.TrackException(filterContext.Exception);
                    }
                }
            base.OnException(filterContext);
            }
        }
    }