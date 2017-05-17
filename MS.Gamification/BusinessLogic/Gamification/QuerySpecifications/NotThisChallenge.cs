using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications
    {
    public class NotThisChallenge : QuerySpecification<Challenge>
        {
        readonly Challenge excludedChallenge;

        public NotThisChallenge(Challenge excludedChallenge)
            {
            this.excludedChallenge = excludedChallenge;
            }

        public override IQueryable<Challenge> GetQuery(IQueryable<Challenge> items)
            {
            return from item in items
                   where item.Id != excludedChallenge.Id
                   select item;
            }
        }
    }