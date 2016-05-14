// This file is part of the MS.Gamification project
// 
// File: IRepository.cs  Created: 2016-05-10@22:28
// Last modified: 2016-05-14@22:29

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;
using MS.Gamification.BusinessLogic;

namespace MS.Gamification.DataAccess
    {
    /// <summary>
    ///     Generic repository interface which must be implemented by all repositories that participate in a Unit of Work.
    /// </summary>
    /// <remarks>
    ///     This generic interface is database and application independent and is one of the key articulation points in
    ///     the application architecture. The interface defines behaviours that are common to all repositories and provide
    ///     the foundation for entity repositories to specialise into a selection of queries and operations required by
    ///     the business logic.
    /// </remarks>
    /// <typeparam name="TEntity">The type of entity contained in the repository.</typeparam>
    public interface IRepository<TEntity, TKey> where TEntity : class, IDomainEntity<TKey>
        {
        /// <summary>
        ///     Gets the pick list (a collection of key-value pairs).
        /// </summary>
        /// <value>The pick list.</value>
        [NotNull]
        IEnumerable<PickListItem<TKey>> PickList { get; }

        /// <summary>
        ///     Gets the specified entity.
        /// </summary>
        /// <param name="id">The entity ID.</param>
        TEntity Get(TKey id);

        /// <summary>
        ///     Gets an enumerable collection of all entities in the entity set.
        /// </summary>
        /// <returns><see cref="IEnumerable{TEntity}" />.</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        ///     Gets all entities that satisfy a <paramref name="predicate" />.
        /// </summary>
        /// <param name="predicate">A predicate expression tree.</param>
        /// <returns>An <see cref="IEnumerable{TEntity}" /> containing all entities that satisfy the predicate.</returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Adds one entity to the entity set.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void Add(TEntity entity);

        /// <summary>
        ///     Adds entities to the entity set.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Add(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Removes one entity from the entity set.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(TEntity entity);

        /// <summary>
        ///     Removes entities from the entity set.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void Remove(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Gets a single entity by ID, if it exists.
        /// </summary>
        /// <param name="id">The entity ID.</param>
        /// <returns>A <see cref="Maybe{T}" /> that either contains the matched entity, or is empty.</returns>
        Maybe<TEntity> GetMaybe(TKey id);

        /// <summary>
        ///     Gets all entities that satisfy the supplied specification.
        /// </summary>
        /// <param name="specification">A specification that determines which entities should be returned.</param>
        /// <returns>A collection of all entities satisfying the specification.</returns>
        IEnumerable<TEntity> AllSatisfying(IQuerySpecification<TEntity> specification);

        /// <summary>
        ///     Gets the single entity that matches the specification.
        ///     Throws an exception if there is not exactly one entity matching the criteria.
        /// </summary>
        /// <param name="specification">A query specification for the desired entity</param>
        /// <returns>The one and only entity that satisfies the criteria.</returns>
        /// <exception cref="InvalidOperationException">Thrown if there is not exactly one match.</exception>
        TEntity Single(IQuerySpecification<TEntity> specification);
        }
    }