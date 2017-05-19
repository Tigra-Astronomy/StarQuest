// This file is part of the MS.Gamification project
// 
// File: ObservingSessionSpecs.cs  Created: 2017-05-17@18:51
// Last modified: 2017-05-18@19:29

using System;
using System.Linq;
using Machine.Specifications;
using MS.Gamification.Areas.Admin.ViewModels.ObservingSessions;
using MS.Gamification.BusinessLogic.EventManagement;

namespace MS.Gamification.Tests.EventLogic
    {
    [Subject(typeof(ObservingSessionLogic), "new session")]
    class when_creating_a_new_observing_session : with_event_logic_context
        {
        Establish context = () => EventContext = EventContextBuilder.Build();
        Because of = () => SessionManager.CreateAsync(new CreateObservingSessionViewModel
                {
                Title = "Test session",
                Venue = "Your imagination",
                StartsAt = new DateTime(2000, 1, 1, 0, 0, 0)
                })
            .Wait();
        It should_create_one_session_in_scheduled_state = () =>
            Sessions.Single().ScheduleState.ShouldEqual(ScheduleState.Scheduled);
        }
    }