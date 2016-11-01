// This file is part of the MS.Gamification project
// 
// File: SingleUserWithObservations.cs  Created: 2016-07-09@20:14
// Last modified: 2016-08-11@07:13

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    public class SingleUserWithObservations : QuerySpecification<ApplicationUser>
        {
        private readonly string requestedUserId;

        public SingleUserWithObservations(string userId)
            {
            requestedUserId = userId;
            FetchStrategy.Include(p => p.Observations.Select(q => q.Challenge));
            }

        public override IQueryable<ApplicationUser> GetQuery(IQueryable<ApplicationUser> items)
            {
            var query = from user in items
                        where user.Id == requestedUserId
                        select user;
            return query;
            }
        }
    }