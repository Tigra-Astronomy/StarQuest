using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QuerySpecifications
    {
    public class SingleUserWithObservations : QuerySpecification<ApplicationUser>
        {
        readonly string userId;

        public SingleUserWithObservations(string userId )
            {
            this.userId = userId;
            FetchStrategy.Include("Observations.Challenge");
            // Eager loading of the user's observations
            }

        public override IQueryable<ApplicationUser> GetQuery(IQueryable<ApplicationUser> items)
            {
            return items.Where(p => p.Id == userId);
            }
        }
    }