using System.Collections.Generic;
using System.Linq;
using MS.Gamification.Models;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    public class BadgeRepository : Repository<Badge, int>
        {
        public BadgeRepository(ApplicationDbContext dbContext) : base(dbContext) {}

        public override IEnumerable<PickListItem<int>> PickList =>
            GetAll().Select(p => new PickListItem<int>(p.Id, p.Name));

        }
    }