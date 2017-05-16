// This file is part of the MS.Gamification project
// 
// File: MyAttribute.cs  Created: 2016-11-25@22:23
// Last modified: 2016-11-25@22:25

using System;
using System.Reflection;
using System.Web.Mvc;

namespace MS.Gamification.HtmlHelpers
    {
    [AttributeUsage(AttributeTargets.Method)]
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
        {
        public string Name { get; set; }

        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
            var isValidName = false;
            var keyValue = string.Format("{0}:{1}", Name, Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

            if (value != null)
                {
                controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                isValidName = true;
                }

            return isValidName;
            }
        }
    }