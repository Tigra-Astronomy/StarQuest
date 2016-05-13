// This file is part of the MS.Gamification project
// 
// File: ApplicationUser.cs  Created: 2016-03-13@22:30
// Last modified: 2016-03-21@22:05 by Fern

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
    public class ApplicationUser : IdentityUser, IDomainEntity<String>
        {
        public ApplicationUser()
            {
            Observations = new List<Observation>();
            }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
            {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
            }

        public virtual List<Observation> Observations { get; set; }
        }
    }
