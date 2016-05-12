using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QuerySpecifications
    {
    public class AllChallengesInCategory: CompositeSpecification<Challenge>
        {
        readonly Category category;

        public AllChallengesInCategory(Category category)
            {
            this.category = category;
            }

        public override bool IsSatisfiedBy(Challenge candidate)
            {
            return (candidate.CategoryId == category.Id);
            }
        }
    }