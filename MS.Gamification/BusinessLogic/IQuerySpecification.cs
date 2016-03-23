// This file is part of the MS.Gamification project
// 
// File: IQuerySpecification.cs  Created: 2016-03-17@01:39
// Last modified: 2016-03-17@02:55 by Fern

namespace MS.Gamification.BusinessLogic
    {
    /// <summary>
    ///   Represents a specification of some subset of a set of objects.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity that the specification applies to.</typeparam>
    public interface IQuerySpecification<in TEntity> where TEntity : class
        {
        /// <summary>
        ///   A predicate that can be applied to each member of a set to determine whether it satisfies the
        ///   specification.
        /// </summary>
        /// <param name="entity">The type of entity being tested.</param>
        /// <returns><c>true</c> if the specified entity satisfies the specification; otherwise, <c>false</c>.</returns>
        bool IsSatisfiedBy(TEntity entity);
        }
    }
