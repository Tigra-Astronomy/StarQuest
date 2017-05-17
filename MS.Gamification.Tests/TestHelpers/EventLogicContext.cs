// This file is part of the MS.Gamification project
// 
// File: EventLogicContext.cs  Created: 2017-05-17@19:20
// Last modified: 2017-05-17@19:20

using MS.Gamification.BusinessLogic.EventManagement;

namespace MS.Gamification.Tests.TestHelpers
    {
    class EventLogicContext {
        public IObservingSessionManager SessionManager { get; private set; }
        }
    }