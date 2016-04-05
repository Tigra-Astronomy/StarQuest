// This file is part of the MS.Gamification project
// 
// File: CategoryRepository.cs  Created: 2016-04-03@23:45
// Last modified: 2016-04-03@23:46 by Fern

using System.Collections.Generic;
using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    public class CategoryRepository : Repository<Category, int>
        {
        public CategoryRepository(ApplicationDbContext context) : base(context) {}

        public override IEnumerable<PickListItem<int>> PickList
            => GetAll().Select(p => new PickListItem<int>(p.Id, p.Name));
        }
    }