using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QuerySpecifications
    {
    public class IncorrectValidationImagesForChallenge : QuerySpecification<Challenge>
        {
        readonly Challenge challenge;
        IQuerySpecification<Challenge> challengesInSameCategory;
        IQuerySpecification<Challenge> notThisChallenge;

        public IncorrectValidationImagesForChallenge(Challenge challenge)
            {
            this.challenge = challenge;
            challengesInSameCategory = new ChallengesInCategory(challenge.Category);
            notThisChallenge = new NotThisChallenge(challenge);
            }


        public override IQueryable<Challenge> Query(IQueryable<Challenge> items)
            {
            var challengesInCategory = challengesInSameCategory.Query(items);
            var result = notThisChallenge.Query(challengesInCategory);
            return result;
            }
        }
    }