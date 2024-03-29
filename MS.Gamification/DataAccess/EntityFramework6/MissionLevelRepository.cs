// This file is part of the MS.Gamification project
// 
// File: MissionLevelRepository.cs  Created: 2016-07-01@20:29
// Last modified: 2016-07-01@20:39

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    internal class MissionLevelRepository : Repository<MissionLevel, int>
        {
        public MissionLevelRepository(DbContext dbContext) : base(dbContext) {}

        public override IEnumerable<PickListItem<int>> PickList
            => GetAll().Select(p => new PickListItem<int>(p.Id, p.Name));
        }
    }