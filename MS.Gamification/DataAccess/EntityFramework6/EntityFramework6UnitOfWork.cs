// This file is part of the MS.Gamification project
// 
// File: EntityFramework6UnitOfWork.cs  Created: 2016-11-01@19:37
// Last modified: 2017-05-16@19:45

using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MS.Gamification.Models;
using NLog;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    public class EntityFramework6UnitOfWork : IUnitOfWork
        {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly ApplicationDbContext dbContext;

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
            ObservingSessions = new ObservingSessionRepository(dbContext);
            }

        [NotNull]
        public IRepository<ObservingSession, int> ObservingSessions { get; }

        [NotNull]
        public IRepository<Challenge, int> Challenges { get; }

        [NotNull]
        public IRepository<ApplicationUser, string> Users { get; }

        [NotNull]
        public IRepository<Category, int> CategoriesRepository { get; }

        [NotNull]
        public IRepository<Observation, int> Observations { get; }

        [NotNull]
        public IRepository<MissionLevel, int> MissionLevels { get; }

        [NotNull]
        public IRepository<Mission, int> Missions { get; }

        [NotNull]
        public IRepository<MissionTrack, int> MissionTracks { get; }

        [NotNull]
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

        [NotNull]
        public Task CommitAsync()
            {
            Contract.Ensures(Contract.Result<Task>() != null);
            try
                {
                return dbContext.SaveChangesAsync();
                }
            catch (Exception e)
                {
                Log.Error(e, "Committing database transaction");
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