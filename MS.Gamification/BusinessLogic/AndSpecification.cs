// This file is part of the MS.Gamification project
// 
// File: AndSpecification.cs  Created: 2016-03-17@01:53
// Last modified: 2016-03-17@02:55 by Fern

namespace MS.Gamification.BusinessLogic
    {
    /// <summary>
    ///   A composite specification representing the logical AND of two other specifications.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to which the specification can be applied.</typeparam>
    /// <seealso cref="CompositeSpecification{TEntity}" />
    class AndSpecification<TEntity> : CompositeSpecification<TEntity> where TEntity : class
        {
        readonly IQuerySpecification<TEntity> left;
        readonly IQuerySpecification<TEntity> right;

        /// <summary>
        ///   Initializes a new instance of the <see cref="AndSpecification{TEntity}" /> class.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        internal AndSpecification(IQuerySpecification<TEntity> left, IQuerySpecification<TEntity> right)
            {
            this.left = left;
            this.right = right;
            }

        /// <summary>
        ///   Determines whether the candidate satisfies both left and right specifications.
        /// </summary>
        /// <param name="candidate">The candidate entity.</param>
        /// <returns><c>true</c> if the candidate specifies both specifications; otherwise, <c>false</c>.</returns>
        public override bool IsSatisfiedBy(TEntity candidate)
            {
            return left.IsSatisfiedBy(candidate) && right.IsSatisfiedBy(candidate);
            }
        }
    }
