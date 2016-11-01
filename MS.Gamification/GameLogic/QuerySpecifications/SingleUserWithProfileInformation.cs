// This file is part of the MS.Gamification project
// 
// File: SingleUserWithProfileInformation.cs  Created: 2016-07-29@16:26
// Last modified: 2016-07-29@16:29

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    public class SingleUserWithProfileInformation : QuerySpecification<ApplicationUser>
        {
        private readonly string userId;

        public SingleUserWithProfileInformation(string userId)
            {
            this.userId = userId;
            FetchStrategy.Include(p => p.Badges);
            FetchStrategy.Include(p => p.Observations);
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