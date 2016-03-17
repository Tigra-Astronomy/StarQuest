// This file is part of the MS.Gamification project
// 
// File: NotSpecification.cs  Created: 2016-03-17@01:57
// Last modified: 2016-03-17@02:55 by Fern

namespace MS.Gamification.BusinessLogic
    {
    /// <summary>
    ///   Composes a specification that represents the inverse (logical NOT) of a specification.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to which the specification can be applied.</typeparam>
    /// <seealso cref="CompositeSpecification{TEntity}" />
    class NotSpecification<TEntity> : CompositeSpecification<TEntity> where TEntity : class
        {
        readonly IQuerySpecification<TEntity> specification;

        /// <summary>
        ///   Initializes a new instance of the <see cref="NotSpecification{TEntity}" /> class.
        /// </summary>
        /// <param name="specification">The source specification to be inverted.</param>
        internal NotSpecification(IQuerySpecification<TEntity> specification)
            {
            this.specification = specification;
            }

        /// <summary>
        ///   Determines whether the candidate entity satisfies the specification.
        /// </summary>
        /// <param name="candidate">The candidate entity being tested.</param>
        /// <returns><c>true</c> if the candidate satisfies the specification; otherwise, <c>false</c>.</returns>
        public override bool IsSatisfiedBy(TEntity candidate)
            {
            return !specification.IsSatisfiedBy(candidate);
            }
        }
    }
