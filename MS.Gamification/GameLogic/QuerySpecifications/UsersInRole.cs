// This file is part of the MS.Gamification project
// 
// File: UsersInRole.cs  Created: 2016-12-13@03:06
// Last modified: 2016-12-13@03:10

using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    /// <summary>
    ///     Matches all the users who have the specified role.
    /// </summary>
    /// <seealso cref="ApplicationUser" />
    internal class UsersInRole : QuerySpecification<ApplicationUser>
        {
        private readonly string role;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersInRole(string role, RoleManager<IdentityRole> roleManager)
            {
            this.role = role;
            this.roleManager = roleManager;
            }

        public override IQueryable<ApplicationUser> GetQuery(IQueryable<ApplicationUser> items)
            {
            var targetRoleId = roleManager.Roles.Single(p => p.Name == role).Id;
            var usersInRole = from user in items
                              where user.Roles.Any(p => p.RoleId == targetRoleId)
                              select user;
            return usersInRole;
            }
        }
    }