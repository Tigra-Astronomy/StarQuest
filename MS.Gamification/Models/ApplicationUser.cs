// This file is part of the MS.Gamification project
// 
// File: ApplicationUser.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-20@13:29

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Models
    {
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser, IDomainEntity<string>
        {
        public virtual List<Observation> Observations { get; set; } = new List<Observation>();

        public virtual List<Badge> Badges { get; set; } = new List<Badge>();

        /// <summary>
        ///     The date and time on which the user account was provisioned.
        /// </summary>
        public DateTime Provisioned { get; set; } = DateTime.Now;

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
            {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
            }
        }
    }