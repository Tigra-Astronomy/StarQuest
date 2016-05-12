using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QuerySpecifications
    {
    public class IncorrectValidationImagesForChallenge : CompositeSpecification<Challenge>
        {
        readonly Challenge challenge;
        IQuerySpecification<Challenge> compositeSpecification;

        public IncorrectValidationImagesForChallenge(Challenge challenge)
            {
            this.challenge = challenge;
            var challengesInSameCategory = new AllChallengesInCategory(challenge.Category);
            var notThisChallenge = new NotThisChallenge(challenge);
            compositeSpecification = challengesInSameCategory.And(notThisChallenge);
            }

        public override bool IsSatisfiedBy(Challenge candidate)
            {
            return compositeSpecification.IsSatisfiedBy(candidate);
            }
        }
    }