// This file is part of the MS.Gamification project
// 
// File: SingleUserWithObservations.cs  Created: 2016-05-14@01:42
// Last modified: 2016-05-14@21:33

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.GameLogic.QuerySpecifications
    {
    public class SingleUserWithObservations : QuerySpecification<ApplicationUser>
        {
        readonly string requestedUserId;

        public SingleUserWithObservations(string userId)
            {
            requestedUserId = userId;
            FetchStrategy.Include("Observations.Challenge");
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