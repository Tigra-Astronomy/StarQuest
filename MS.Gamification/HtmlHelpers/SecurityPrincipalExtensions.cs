// This file is part of the MS.Gamification project
// 
// File: SecurityPrincipalExtensions.cs  Created: 2017-06-20@16:20
// Last modified: 2017-06-20@16:26

using System.Security.Principal;
using MS.Gamification.Models;

namespace MS.Gamification.HtmlHelpers
    {
    /// <summary>
    ///     Extension methods intended for use in Razor views for checking user capabilities.
    /// </summary>
    public static class SecurityPrincipalExtensions
        {
        public static bool CanManageEvents(this IPrincipal user) => user.IsInRole(RoleNames.EventManager) || user.CanAdminister();

        public static bool CanModerate(this IPrincipal user) => user.IsInRole(RoleNames.Moderator) || user.CanAdminister();

        public static bool CanAdminister(this IPrincipal user) => user.IsInRole(RoleNames.Administrator);
        }
    }