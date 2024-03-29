﻿// This file is part of the MS.Gamification project
// 
// File: EntityFramework6UnitOfWork.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-20@22:39

using System;
using System.Threading.Tasks;
using MS.Gamification.Models;
using NLog;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    public class EntityFramework6UnitOfWork : IUnitOfWork
        {
        private readonly ApplicationDbContext dbContext;
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        public EntityFramework6UnitOfWork(ApplicationDbContext context)
            {
            dbContext = context;
            Users = new UserRepository(dbContext);
            Challenges = new ChallengeRepository(dbContext);
            CategoriesRepository = new CategoryRepository(dbContext);
            Observations = new ObservationRepository(dbContext);
            MissionLevels = new MissionLevelRepository(dbContext);
            Missions = new MissionRepository(dbContext);
            MissionTracks = new MissionTrackRepository(dbContext);
            Badges = new BadgeRepository(dbContext);
            }

        public IRepository<Challenge, int> Challenges { get; }

        public IRepository<ApplicationUser, string> Users { get; }

        public IRepository<Category, int> CategoriesRepository { get; }

        public IRepository<Observation, int> Observations { get; }

        public IRepository<MissionLevel, int> MissionLevels { get; }

        public IRepository<Mission, int> Missions { get; }

        public IRepository<MissionTrack, int> MissionTracks { get; }

        public IRepository<Badge, int> Badges { get; }

        public void Commit()
            {
            try
                {
                dbContext.SaveChanges();
                }
            catch (Exception e)
                {
                Log.Error(e, "Committing database transaction");
                throw;
                }
            }

        public Task CommitAsync()
            {
            try
                {
                return dbContext.SaveChangesAsync();
                }
            catch (Exception e)
                {
                Log.Error(e,"Committing database transaction");
                throw;
                }
            }

        public void Cancel()
            {
            Log.Error("Canceling database transaction");
            dbContext.Dispose();
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