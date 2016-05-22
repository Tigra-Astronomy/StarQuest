// This file is part of the MS.Gamification project
// 
// File: IUnitOfWork.cs  Created: 2016-05-10@22:28
// Last modified: 2016-05-22@06:44

using System;
using MS.Gamification.Models;

namespace MS.Gamification.DataAccess
    {
    /// <summary>
    ///     Represents a database transaction
    /// </summary>
    public interface IUnitOfWork : IDisposable
        {
        /// <summary>
        ///     Gets the challenges repository.
        /// </summary>
        /// <value>The challenges repository.</value>
        IRepository<Challenge, int> ChallengesRepository { get; }

        /// <summary>
        ///     Gets the users repository.
        /// </summary>
        /// <value>The users.</value>
        IRepository<ApplicationUser, string> UsersRepository { get; }

        /// <summary>
        ///     Gets the categories repository.
        /// </summary>
        /// <value>The categories repository.</value>
        IRepository<Category, int> CategoriesRepository { get; }

        /// <summary>
        ///     Gets the observations repository.
        /// </summary>
        /// <value>The observations repository.</value>
        IRepository<Observation, int> ObservationsRepository { get; }

        /// <summary>
        ///     Commits changes to the database and completes the transaction.
        /// </summary>
        void Commit();

        /// <summary>
        ///     Cancels the transaction and undoes any pending changes.
        /// </summary>
        void Cancel();
        }
    }