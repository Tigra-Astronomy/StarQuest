// This file is part of the MS.Gamification project
// 
// File: SingleUserWithBadges.cs  Created: 2016-07-26@12:42
// Last modified: 2016-07-26@12:45

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications
    {
    public class SingleUserWithBadges : QuerySpecification<ApplicationUser>
        {
        private readonly string userId;

        public SingleUserWithBadges(string userId)
            {
            this.userId = userId;
            FetchStrategy.Include(p => p.Badges);
            }

        public override IQueryable<ApplicationUser> GetQuery(IQueryable<ApplicationUser> items)
            {
            var query = from user in items
                        where user.Id == userId
                        select user;
            return query;
            }
        }
    }