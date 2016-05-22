// This file is part of the MS.Gamification project
// 
// File: EffortUnitOfWorkBuilder.cs  Created: 2016-05-22@04:50
// Last modified: 2016-05-22@06:29

using Effort;
using Effort.DataLoaders;
using Effort.Extra;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.DataAccess;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;

namespace MS.Gamification.Tests.TestHelpers
    {
    /// <summary>
    ///     Builds an <see cref="IUnitOfWork" /> based on the EFFORT in-memory database provider.
    /// </summary>
    class EffortUnitOfWorkBuilder
        {
        IDataLoader loader;

        public EffortUnitOfWorkBuilder WithData(IDataLoader loader)
            {
            this.loader = loader;
            return this;
            }

        public IUnitOfWork Build()
            {
            var dataLoader = loader ?? new ObjectDataLoader(new ObjectData());
            var connection = DbConnectionFactory.CreateTransient(dataLoader);
            var dbContext = new ApplicationDbContext(connection);
            var uow = new EntityFramework6UnitOfWork(dbContext,
                new ApplicationUserManager(new UserStore<ApplicationUser>(dbContext)));
            return uow;
            }
        }
    }