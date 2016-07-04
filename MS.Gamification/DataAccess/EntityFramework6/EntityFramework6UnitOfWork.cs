// This file is part of the MS.Gamification project
// 
// File: EntityFramework6UnitOfWork.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-04@00:53

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
            UsersRepository = new UserRepository(dbContext);
            Challenges = new ChallengeRepository(dbContext);
            CategoriesRepository = new CategoryRepository(dbContext);
            Observations = new ObservationRepository(dbContext);
            Missions = new MissionRepository(dbContext);
            }

        public IRepository<Challenge, int> Challenges { get; }

        public IRepository<ApplicationUser, string> UsersRepository { get; }

        public IRepository<Category, int> CategoriesRepository { get; }

        public IRepository<Observation, int> Observations { get; }

        public IRepository<Mission, int> Missions { get; }

        public void Commit()
            {
            try
                {
                dbContext.SaveChanges();
                }
            catch (Exception e)
                {
                Console.WriteLine(e); // ToDo: write the exception to a log file
                throw;
                }
            }

        public void Cancel()
            {
            throw new NotImplementedException();
            }

        #region IDisposable Pattern
        // The IDisposable pattern, as described at
        // http://www.codeproject.com/Articles/15360/Implementing-IDisposable-and-the-Dispose-Pattern-P


        /// <summary>
        ///     Finalizes this instance (called prior to garbage collection by the CLR)
        /// </summary>
        ~EntityFramework6UnitOfWork()
            {
            Dispose(false);
            }

        /// <summary>
        ///     Disposes the unit of work, discarding any uncommitted repository modifications.
        /// </summary>
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }

        private bool disposed;

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="fromUserCode">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to
        ///     release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool fromUserCode)
            {
            if (!disposed)
                {
                if (fromUserCode)
                    {
                    // ToDo - Dispose managed resources (call Dispose() on any owned objects).
                    // Do not dispose of any objects that may be referenced elsewhere.
                    dbContext.Dispose();
                    }

                // ToDo - Release unmanaged resources here, if necessary.
                }
            disposed = true;
            }
        #endregion
        }
    }