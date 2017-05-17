// This file is part of the MS.Gamification project
// 
// File: with_event_logic_context.cs  Created: 2017-05-17@19:24
// Last modified: 2017-05-17@19:51

using Machine.Specifications;
using MS.Gamification.BusinessLogic.EventManagement;
using MS.Gamification.Tests.TestHelpers;

namespace MS.Gamification.Tests.EventLogic
    {
    class with_event_logic_context
        {
        protected static EventLogicContextBuilder EventContextBuilder;
        protected static EventLogicContext EventContext;
        Cleanup after = () =>
            {
            EventContext = null;
            EventContextBuilder = null;
            };
        Establish context = () => EventContextBuilder = new EventLogicContextBuilder();

        protected static IObservingSessionManager SessionManager => EventContext.SessionManager;
        }
    }