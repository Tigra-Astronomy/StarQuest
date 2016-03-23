// This file is part of the MS.Gamification project
// 
// File: OrSpecification.cs  Created: 2016-03-17@01:58
// Last modified: 2016-03-17@02:55 by Fern

namespace MS.Gamification.BusinessLogic
    {
    /// <summary>
    ///   A composite specification representing the logical OR of two other specifications.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <seealso cref="CompositeSpecification{TEntity}" />
    class OrSpecification<TEntity> : CompositeSpecification<TEntity> where TEntity : class
        {
        readonly IQuerySpecification<TEntity> left;
        readonly IQuerySpecification<TEntity> right;

        /// <summary>
        ///   Initializes a new instance of the <see cref="OrSpecification{TEntity}" /> class.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        internal OrSpecification(IQuerySpecification<TEntity> left, IQuerySpecification<TEntity> right)
            {
            this.left = left;
            this.right = right;
            }

        /// <summary>
        ///   Determines whether the candidate satisfies either the left child specification OR the right child
        ///   specification.
        /// </summary>
        /// <param name="candidate">The candidate entity.</param>
        /// <returns><c>true</c> if the candidate satisfies the specification; otherwise, <c>false</c>.</returns>
        public override bool IsSatisfiedBy(TEntity candidate)
            {
            return left.IsSatisfiedBy(candidate) || right.IsSatisfiedBy(candidate);
            }
        }
    }
