// This file is part of the MS.Gamification project
// 
// File: SingleObservationWithNavigationProperties.cs  Created: 2016-05-26@01:59
// Last modified: 2016-05-26@02:04

using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.QuerySpecifications
    {
    public class SingleObservationWithNavigationProperties : QuerySpecification<Observation>
        {
        readonly int observationId;

        public SingleObservationWithNavigationProperties(int observationId)
            {
            this.observationId = observationId;
            FetchStrategy.Include(p => p.Challenge);
            FetchStrategy.Include(p => p.User);
            }

        public override IQueryable<Observation> GetQuery(IQueryable<Observation> items)
            {
            return from item in items
                   where item.Id == observationId
                   select item;
            }
        }
    }