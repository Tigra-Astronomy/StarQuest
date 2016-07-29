// This file is part of the MS.Gamification project
// 
// File: WebApiConfig.cs  Created: 2016-07-29@21:31
// Last modified: 2016-07-29@21:32

using System.Web.Http;
using Newtonsoft.Json.Serialization;

namespace MS.Gamification
    {
    public static class WebApiConfig
        {
        public static void Register(HttpConfiguration config)
            {
            // TODO: Add any additional configuration code.

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
                );

            // WebAPI when dealing with JSON & JavaScript!
            // Setup json serialization to serialize classes to camel (std. Json format)
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            }
        }
    }