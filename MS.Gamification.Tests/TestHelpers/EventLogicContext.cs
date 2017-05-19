// This file is part of the MS.Gamification project
// 
// File: EventLogicContext.cs  Created: 2017-05-17@19:20
// Last modified: 2017-05-18@16:34

using AutoMapper;
using MS.Gamification.BusinessLogic.EventManagement;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.DataAccess;

namespace MS.Gamification.Tests.TestHelpers
    {
    class EventLogicContext
        {
        public IObservingSessionManager SessionManager { get; set; }

        public IUnitOfWork UnitOfWork { get; set; }

        public IMapper Mapper { get; set; }

        public IGameNotificationService Notifier { get; set; }
        }
    }