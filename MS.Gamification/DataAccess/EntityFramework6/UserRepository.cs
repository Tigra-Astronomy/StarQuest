// This file is part of the MS.Gamification project
// 
// File: UserRepository.cs  Created: 2016-03-17@01:31
// Last modified: 2016-03-17@02:54 by Fern

using System;
using Microsoft.AspNet.Identity;
using MS.Gamification.Models;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    public class UserRepository : Repository<ApplicationUser>
        {
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
            : base(dbContext)
            {
            this.userManager = userManager;
            throw new NotImplementedException();
            }
        }
    }
