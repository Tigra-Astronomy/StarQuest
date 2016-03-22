// This file is part of the MS.Gamification project
// 
// File: IUnitOfWork.cs  Created: 2016-03-18@20:18
// Last modified: 2016-03-21@22:36 by Fern

using MS.Gamification.Models;

namespace MS.Gamification.DataAccess
    {
    /// <summary>
    ///   Represents a database transaction
    /// </summary>
    public interface IUnitOfWork
        {
        /// <summary>
        ///   Gets the challenges repository.
        /// </summary>
        /// <value>The challenges repository.</value>
        IRepository<Challenge, int> ChallengesRepository { get; }

        /// <summary>
        ///   Gets the users repository.
        /// </summary>
        /// <value>The users.</value>
        IRepository<ApplicationUser, string> UsersRepository { get; }

        /// <summary>
        ///   Commits changes to the database and completes the transaction.
        /// </summary>
        void Commit();

        /// <summary>
        ///   Cancels the transaction and undoes any pending changes.
        /// </summary>
        void Cancel();
        }
    }
