// This file is part of the MS.Gamification project
// 
// File: EntityFramework6UnitOfWork.cs  Created: 2016-03-17@01:10
// Last modified: 2016-03-17@02:54 by Fern

using System;
using Microsoft.AspNet.Identity;
using MS.Gamification.Models;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    public class EntityFramework6UnitOfWork : IUnitOfWork
        {
        private readonly ApplicationDbContext dbContext;

        public EntityFramework6UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
            {
            dbContext = context;
            Users = new UserRepository(dbContext, userManager);
            Challenges = new ChallengeRepository(dbContext);
            }

        public IRepository<Challenge> Challenges { get; }
        public IRepository<ApplicationUser> Users { get; }

        public void Commit()
            {
            throw new NotImplementedException();
            }

        public void Cancel()
            {
            throw new NotImplementedException();
            }
        }
    }
