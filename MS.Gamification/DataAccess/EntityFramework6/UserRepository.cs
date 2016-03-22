// This file is part of the MS.Gamification project
// 
// File: UserRepository.cs  Created: 2016-03-18@20:18
// Last modified: 2016-03-21@22:48 by Fern

using Microsoft.AspNet.Identity;
using MS.Gamification.Models;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    /// <summary>
    ///   Stores all of the registered users, their passwords, claims and other details.
    /// </summary>
    /// <seealso cref="EntityFramework6.Repository{ApplicationUser, string}" />
    public class UserRepository : Repository<ApplicationUser, string>
        {
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        ///   Initializes a new instance of the <see cref="UserRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userManager">The user manager.</param>
        public UserRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
            : base(dbContext)
            {
            this.userManager = userManager;
            }
        }
    }
