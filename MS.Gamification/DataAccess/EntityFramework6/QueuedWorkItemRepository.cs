using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;
using MS.Gamification.BusinessLogic.QueueProcessing;
using MS.Gamification.Models;

namespace MS.Gamification.DataAccess.EntityFramework6 {
    public class QueuedWorkItemRepository : Repository<QueuedWorkItem, int>
        {
        public QueuedWorkItemRepository([NotNull] ApplicationDbContext dbContext) : base(dbContext)
            {
            Contract.Requires(dbContext != null);
            }

        public override IEnumerable<PickListItem<int>> PickList => Enumerable.Empty<PickListItem<int>>();
        }
    }