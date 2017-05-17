// This file is part of the MS.Gamification project
// 
// File: ObservingSessionSpecs.cs  Created: 2017-05-17@18:51
// Last modified: 2017-05-17@19:51

using System;
using Machine.Specifications;
using MS.Gamification.Areas.Admin.ViewModels.ObservingSessions;
using MS.Gamification.BusinessLogic.EventManagement;

namespace MS.Gamification.Tests.EventLogic
    {
    [Subject(typeof(ObservingSessionLogic), "new session")]
    class when_creating_a_new_observing_session : with_event_logic_context
        {
        Establish context = () => EventContext = EventContextBuilder.Build();
        Because of = () => SessionManager.Create(new CreateObservingSessionViewModel
            {
            Title = "Test session",
            Venue = "Your imagination",
            StartsAt = new DateTime(2000, 1, 1, 0, 0, 0)
            });
        It should_create_a_session_in_scheduled_state;
        }
    }