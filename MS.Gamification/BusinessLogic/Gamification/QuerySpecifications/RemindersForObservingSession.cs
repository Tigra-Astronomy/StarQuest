using System.Diagnostics.Contracts;
using System.Linq;
using JetBrains.Annotations;
using MS.Gamification.BusinessLogic.QueueProcessing;
using MS.Gamification.Models;

namespace MS.Gamification.BusinessLogic.Gamification.QuerySpecifications {
    internal class RemindersForObservingSession : QuerySpecification<QueuedWorkItem, ObservingSessionReminder>
        {
        private readonly int sessionId;

        public RemindersForObservingSession(int sessionId)
            {
            this.sessionId = sessionId;
            }

        [ItemNotNull]
        [NotNull]
        public override IQueryable<ObservingSessionReminder> GetQuery([ItemNotNull] [NotNull] IQueryable<QueuedWorkItem> items)
            {
            Contract.Requires(items != null);
            Contract.Ensures(Contract.Result<IQueryable<ObservingSessionReminder>>() != null);
            var query = items.OfType<ObservingSessionReminder>()
                .Where(p => p.ObservingSessionId == sessionId);
            return query;
            }
        }
    }