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

        public UsersInRole(string role, RoleManager<IdentityRole> roleManager )
            {
            this.role = role;
            this.roleManager = roleManager;
            }

        public override IQueryable<ApplicationUser> GetQuery(IQueryable<ApplicationUser> items)
            {
            var usersInRole = from identityRole in roleManager.Roles
                              where identityRole.Name == role
                              from roleUser in identityRole.Users
                              join user in items on roleUser.UserId equals user.Id
                              select user;
            return usersInRole;
            }
    }
    }