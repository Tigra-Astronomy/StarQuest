using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications
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


        public override IQueryable<Challenge> GetQuery(IQueryable<Challenge> items)
            {
            var challengesInCategory = challengesInSameCategory.GetQuery(items);
            var result = notThisChallenge.GetQuery(challengesInCategory);
            return result;
            }
        }
    }