// This file is part of the MS.Gamification project
// 
// File: ExampleQuerySpecifications.cs  Created: 2016-03-17@02:02
// Last modified: 2016-03-17@02:55 by Fern

using MS.Gamification.DataAccess;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QuerySpecifications
    {
    /// <summary>
    ///   An example specification
    /// </summary>
    public class ChallengesInCategory : CompositeSpecification<Challenge>
        {
        private readonly string category;

        public ChallengesInCategory(string category)
            {
            this.category = category;
            }

        public override bool IsSatisfiedBy(Challenge entity)
            {
            return entity.Category == category;
            }
        }

    public class ChallengesAtOrAboveLevel : CompositeSpecification<Challenge>
        {
        private readonly int level;

        public ChallengesAtOrAboveLevel(int level)
            {
            this.level = level;
            }

        public override bool IsSatisfiedBy(Challenge entity)
            {
            return entity.Points >= level;
            }
        }

    public class SpecificationUsageExamples
        {
        /// <summary>
        ///   Shows how to compose two specifications
        /// </summary>
        public static IQuerySpecification<Challenge> AdvancedMoonChallenges
            {
            get
                {
                var moonChallenges = new ChallengesInCategory("Moon");
                var advancedChallenges = new ChallengesAtOrAboveLevel(3);
                var advancedMoonChallenges = moonChallenges.And(advancedChallenges);
                return advancedMoonChallenges;
                }
            }

        public void DoSomethingWithAdvancedMoonChallenges(IRepository<Challenge> challenges)
            {
            var advncedChallenges = challenges.AllSatisfying(AdvancedMoonChallenges);
            foreach (var challenge in advncedChallenges)
                {
                // Do something
                }
            }
        }
    }
