// This file is part of the MS.Gamification project
// 
// File: ValidationImagePickList.cs  Created: 2016-08-19@00:52
// Last modified: 2016-08-19@01:08

using System.Linq;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications
    {
    public class ValidationImagePickList : QuerySpecification<Challenge, PickListItem<string>>
        {
        public override IQueryable<PickListItem<string>> GetQuery(IQueryable<Challenge> items)
            {
            return from challenge in items
                   orderby challenge.ValidationImage ascending
                   select new PickListItem<string>
                       {
                       Id = challenge.ValidationImage,
                       DisplayName = challenge.ValidationImage
                       };
            }
        }
    }