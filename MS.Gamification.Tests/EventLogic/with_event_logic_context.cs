// This file is part of the MS.Gamification project
// 
// File: with_event_logic_context.cs  Created: 2017-05-17@19:24
// Last modified: 2017-05-18@18:32

using System.Collections.Generic;
using JetBrains.Annotations;
using Machine.Specifications;
using MS.Gamification.BusinessLogic.EventManagement;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.EventLogic
    {
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    class with_event_logic_context
        {
        [NotNull] protected static EventLogicContextBuilder EventContextBuilder = new EventLogicContextBuilder();
        [CanBeNull] protected static EventLogicContext EventContext;
        Cleanup after = () =>
            {
            EventContext = null;
            EventContextBuilder = null;
            };

        #region Convenience Properties
        protected static IObservingSessionManager SessionManager => EventContext.SessionManager;

        protected static IUnitOfWork UnitOfWork => EventContext.UnitOfWork;

        protected static IEnumerable<ObservingSession> Sessions => EventContext.UnitOfWork.ObservingSessions.GetAll();
        #endregion Convenience Properties
        }
    }