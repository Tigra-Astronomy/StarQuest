// This file is part of the MS.Gamification project
// 
// File: with_event_logic_context.cs  Created: 2017-05-17@19:24
// Last modified: 2017-05-18@18:32

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Machine.Specifications;
using MS.Gamification.BusinessLogic.EventManagement;
using MS.Gamification.BusinessLogic.QueueProcessing;
using MS.Gamification.DataAccess;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.EventLogic
    {
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    class with_event_logic_context
        {
        [NotNull] protected static EventLogicContextBuilder EventContextBuilder ;
        [CanBeNull] protected static EventLogicContext EventContext;
        Establish context = () =>
            {
            EventContextBuilder = new EventLogicContextBuilder();
            };
        Cleanup after = () =>
            {
            EventContext = null; //[Sentinel]
            EventContextBuilder = null; //[Sentinel]
            };

        #region Convenience Properties
        protected static IObservingSessionManager SessionManager => EventContext.SessionManager;

        protected static IUnitOfWork UnitOfWork => EventContext.UnitOfWork;

        protected static IEnumerable<ObservingSessionReminder> Reminders =>
            UnitOfWork.QueuedWorkItems.GetAll()
                .Where(p => p.GetType() == typeof(ObservingSessionReminder))
                .Select(p => p as ObservingSessionReminder);

        protected static IEnumerable<ObservingSession> Sessions => EventContext.UnitOfWork.ObservingSessions.GetAll();
        #endregion Convenience Properties
        }
    }