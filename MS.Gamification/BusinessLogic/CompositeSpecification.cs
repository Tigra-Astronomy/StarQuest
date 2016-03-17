// This file is part of the MS.Gamification project
// 
// File: CompositeSpecification.cs  Created: 2016-03-17@01:54
// Last modified: 2016-03-17@02:55 by Fern

namespace MS.Gamification.BusinessLogic
    {
    /// <summary>
    ///   A specification that can be logically combined with other specifications to form an expression tree.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to which the specification can be applied.</typeparam>
    /// <seealso cref="IQuerySpecification{TEntity}" />
    public abstract class CompositeSpecification<TEntity> : IQuerySpecification<TEntity> where TEntity : class
        {
        /// <summary>
        ///   When overridden in a derived class, determines whether the candidate entity satisfies the specification.
        /// </summary>
        /// <param name="candidate">The candidate entity.</param>
        /// <returns><c>true</c> if the candidate satisfies the specification; otherwise, <c>false</c>.</returns>
        public abstract bool IsSatisfiedBy(TEntity candidate);

        /// <summary>
        ///   Composes a specification that is the logical AND of this specification and another.
        /// </summary>
        /// <param name="other">The other specification being composed.</param>
        /// <returns>
        ///   An <see cref="IQuerySpecification{TEntity}" /> that represents the logical AND of this specification and another.
        /// </returns>
        public IQuerySpecification<TEntity> And(IQuerySpecification<TEntity> other)
            {
            return new AndSpecification<TEntity>(this, other);
            }

        /// <summary>
        ///   Composes a specification that is the logical OR of this specification and another.
        /// </summary>
        /// <param name="other">The other specification being composed.</param>
        /// <returns>
        ///   An <see cref="IQuerySpecification{TEntity}" /> that represents the logical OR of this specification and another.
        /// </returns>
        public IQuerySpecification<TEntity> Or(IQuerySpecification<TEntity> other)
            {
            return new OrSpecification<TEntity>(this, other);
            }

        /// <summary>
        ///   Composes a specification that is the inverse (logical NOT) of this specification.
        /// </summary>
        /// <returns>
        ///   An <see cref="IQuerySpecification{TEntity}" /> that represents the inverse (logical NOT) of this specification.
        /// </returns>
        public IQuerySpecification<TEntity> Not()
            {
            return new NotSpecification<TEntity>(this);
            }
        }
    }
