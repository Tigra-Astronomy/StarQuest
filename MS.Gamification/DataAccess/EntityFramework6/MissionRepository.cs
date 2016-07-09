using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    public class MissionRepository : Repository<Mission, int>
        {
        public MissionRepository(DbContext dbContext) : base(dbContext) {}

        public override IEnumerable<PickListItem<int>> PickList
            => GetAll().Select(p => new PickListItem<int>(p.Id, p.Title));
        }
    }