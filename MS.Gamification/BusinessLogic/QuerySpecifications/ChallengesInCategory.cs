using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QuerySpecifications
    {
    public class ChallengesInCategory: QuerySpecification<Challenge>
        {
        readonly Category category;

        public ChallengesInCategory(Category category)
            {
            this.category = category;
            }

        public override IQueryable<Challenge> GetQuery(IQueryable<Challenge> challenges)
            {
            return from challenge in challenges
                   where challenge.CategoryId == category.Id
                   select challenge;
            }
        }
    }