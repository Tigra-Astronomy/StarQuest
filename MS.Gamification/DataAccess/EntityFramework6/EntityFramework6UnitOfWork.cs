﻿// This file is part of the MS.Gamification project
// 
// File: EntityFramework6UnitOfWork.cs  Created: 2016-04-01@23:54
// Last modified: 2016-04-04@02:06 by Fern

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
            CategoriesRepository = new CategoryRepository(dbContext);
            }

        public IRepository<Challenge, int> ChallengesRepository { get; }
        public IRepository<ApplicationUser, string> UsersRepository { get; }
        public IRepository<Category, int> CategoriesRepository { get; }

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
