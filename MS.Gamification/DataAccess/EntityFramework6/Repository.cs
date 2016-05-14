// This file is part of the MS.Gamification project
// 
// File: Repository.cs  Created: 2016-05-10@22:28
// Last modified: 2016-05-14@02:08

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using MS.Gamification.BusinessLogic;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    /// <summary>
    ///     Generic repository base class with common accessors.
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The type of entity contained by the repository.
    /// </typeparam>
    /// <typeparam name="TKey">
    ///     The type of the primary key
    /// </typeparam>
    /// <remarks>
    ///     <para>
    ///         This abstract base class implements the generic
    ///         <see cref="IRepository{TEntity,TKey}" /> interface and provides implementations for common operations that
    ///         should be available for all entity sets.
    ///     </para>
    ///     <para>
    ///         The implementation is coupled to the database technology being used, so if the persistence framework is
    ///         changed or upgraded then this class will have to be updated or replaced with a new implementation. This will
    ///         likely also affect all derived classes, which will also have to be updated or replaced.
    ///     </para>
    /// </remarks>
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IDomainEntity<TKey>
        {
        /// <summary>
        ///     The database context that will be used to persist and retrieve entities from permanent storage.
        /// </summary>
        protected readonly DbContext Context;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repository{TEntity,TKey}" /> class.
        /// </summary>
        /// <param name="context">
        ///     The database context that will be used to persist and retrieve entities from permanent
        ///     storage.
        /// </param>
        protected Repository(DbContext context)
            {
            Context = context;
            }

        #region Implementation of IRepository<TEntity>
        /// <summary>
        ///     Gets the specified entity.
        /// </summary>
        /// <param name="id">The entity ID.</param>
        /// <returns>
        ///     Returns the entity with the specified primary key. If no
        ///     matching item was found, returns null.
        /// </returns>
        public TEntity Get(TKey id)
            {
            return Context.Set<TEntity>().Find(id);
            }

        /// <summary>
        ///     Gets a single entity by ID, if it exists.
        /// </summary>
        /// <param name="id">The entity ID.</param>
        /// <returns>A <see cref="Maybe{T}" /> that either contains the matched entity, or is empty.</returns>
        public Maybe<TEntity> GetMaybe(TKey id)
            {
            try
                {
                var found = Context.Set<TEntity>().Find(id);
                return found == null ? Maybe<TEntity>.Empty : new Maybe<TEntity>(found);
                }
            catch (Exception)
                {
                return Maybe<TEntity>.Empty;
                }
            }

        /// <summary>
        ///     Gets an enumerable collection of all entities in the entity set.
        /// </summary>
        /// <returns><see cref="IEnumerable{TEntity}" />.</returns>
        public IEnumerable<TEntity> GetAll()
            {
            return Context.Set<TEntity>().ToList();
            }

        /// <summary>
        ///     Gets all entities that satisfy a <paramref name="predicate" />.
        /// </summary>
        /// <param name="predicate">A predicate expression tree.</param>
        /// <returns>An <see cref="IEnumerable{TEntity}" /> containing all entities that satisfy the predicate.</returns>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
            {
            return Context.Set<TEntity>().Where(predicate);
            }

        /// <summary>
        ///     Gets all entities that satisfy the supplied specification. If a <see cref="IFetchStrategy{TEntity}" /> is
        ///     present, then the specified related entities are loaded eagerly.
        /// </summary>
        /// <param name="specification">A specification that determines which entities should be returned.</param>
        /// <returns>A collection of all entities satisfying the specification.</returns>
        public IEnumerable<TEntity> AllSatisfying(IQuerySpecification<TEntity> specification)
            {
            var query = QueryWithFetchStrategy(specification);
            return query.ToList(); // Materialize the query to an enumerable list.
            }

        /// <summary>
        ///     Builds an <see cref="ObjectQuery{TEntity}" /> that takes account of the eager loading fetch strategy.
        /// </summary>
        /// <param name="specification"></param>
        /// <returns>A query that includes eager loading of specified related entities.</returns>
        IQueryable<TEntity> QueryWithFetchStrategy(IQuerySpecification<TEntity> specification)
            {
            var query = specification.GetQuery(Context.Set<TEntity>());
            foreach (var includePath in specification.FetchStrategy.IncludePaths)
                {
                query = query.Include(includePath);
                }
            return query;
            }

        /// <summary>
        ///     Gets a single entity that matches a specification. Throws an exception if there is not exactly one match.
        /// </summary>
        /// <param name="specification">A query specification that should resolve to exactly one match.</param>
        /// <returns>The single matched entity.</returns>
        public TEntity Single(IQuerySpecification<TEntity> specification)
            {
            var query = QueryWithFetchStrategy(specification);
            return query.Single();
            }

        /// <summary>
        ///     Adds one entity to the entity set and ensures that it has a unique identifier.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void Add(TEntity entity)
            {
            Context.Set<TEntity>().Add(entity);
            }

        /// <summary>
        ///     Adds entities to the entity set and ensures that they each have a unique identifier.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void Add(IEnumerable<TEntity> entities)
            {
            foreach (var entity in entities)
                Add(entity);
            }

        /// <summary>
        ///     Removes one entity from the entity set.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public void Remove(TEntity entity)
            {
            Context.Set<TEntity>().Remove(entity);
            }

        /// <summary>
        ///     Removes entities from the entity set.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void Remove(IEnumerable<TEntity> entities)
            {
            Context.Set<TEntity>().RemoveRange(entities);
            }

        /// <summary>
        ///     Gets the pick list (a collection of key-value pairs).
        /// </summary>
        /// <value>The pick list, which can be empty but not null.</value>
        public abstract IEnumerable<PickListItem<TKey>> PickList { get; }
        #endregion
        }
    }