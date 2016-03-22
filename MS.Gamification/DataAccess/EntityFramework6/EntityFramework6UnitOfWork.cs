// This file is part of the MS.Gamification project
// 
// File: EntityFramework6UnitOfWork.cs  Created: 2016-03-18@20:18
// Last modified: 2016-03-21@22:04 by Fern

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
            UsersRepository = new UserRepository(dbContext, userManager);
            ChallengesRepository = new ChallengeRepository(dbContext);
            }

        public IRepository<Challenge, int> ChallengesRepository { get; }
        public IRepository<ApplicationUser, string> UsersRepository { get; }

        public void Commit()
            {
            dbContext.SaveChanges();
            }

        public void Cancel()
            {
            throw new NotImplementedException();
            }
        }
    }
