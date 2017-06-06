// This file is part of the MS.Gamification project
// 
// File: ObservingSessionSpecs.cs  Created: 2017-05-17@18:51
// Last modified: 2017-05-31@13:02

using System;
using System.Linq;
using Machine.Specifications;
using MS.Gamification.Areas.Admin.ViewModels.ObservingSessions;
using MS.Gamification.BusinessLogic.EventManagement;

namespace MS.Gamification.Tests.EventLogic
    {
    /*
     * All about event logic
     * =====================
     * 
     * The system shall have a user role called Event Organizer.
     * 
     * An Event Organizer should be able to create an observing session event, that is
     * scheduled for some time in the future. At the time of creation, the organizer can
     * request that reminder emails be sent out immediately, one week before the event,
     * and one day before the event. When these options are enabled, reminders should be queued
     * for later processing.
     * 
     * When an event/session is created, it becomes "scheduled".
     * 
     * A "scheduled" session may later be cancelled, and this should also cancel any
     * queued reminders for that event. Conversely, the observing session reminder queue
     * processor should not send reminders for an event that has been cancelled.
     * 
     * A scheduled session may be changed to "In Progress" by an Event Organizer.
     * An "In Progress" event may be used for visitor registration and tracking
     * observations for the event.
     */
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

    // Observing session with announcement should queue a reminder for the current date/time
    [Subject(typeof(ObservingSessionLogic), "reminders")]
    class when_creating_a_session_with_an_immediate_announcement : with_event_logic_context
        {
        Establish context = () => EventContext = EventContextBuilder
            .WithCurrentDateTime(CurrentDateTime)
            .Build();
        Because of = () => SessionManager.CreateAsync(model).Wait();
        It should_queue_a_reminder_for_the_current_date_time = () =>
            Reminders.Single().ProcessAfter.ShouldEqual(CurrentDateTime);
        static CreateObservingSessionViewModel model = new CreateObservingSessionViewModel
            {
            Description = "Unit test",
            Venue = "nowhere",
            Title = "Unite Test Event",
            StartsAt = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            SendAnnouncement = true, RemindOneWeekBefore = false, RemindOneDayBefore = false
            };
        static readonly DateTime CurrentDateTime = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

    // Observing session with 1-week reminder should queue a reminder 1-week before the start date/time
    [Subject(typeof(ObservingSessionLogic), "reminders")]
    internal class when_creating_a_session_with_a_1_week_reminder : with_event_logic_context
        {
        Establish context = () => EventContext = EventContextBuilder
            .WithCurrentDateTime(CurrentDateTime)
            .Build();
        Because of = () => SessionManager.CreateAsync(model).Wait();
        It should_queue_a_reminder_for_one_week_before_the_event = () =>
            Reminders.Single().ProcessAfter.ShouldEqual(model.StartsAt - TimeSpan.FromDays(7));
        static CreateObservingSessionViewModel model = new CreateObservingSessionViewModel
            {
            Description = "Unit test",
            Venue = "nowhere",
            Title = "Unite Test Event",
            StartsAt = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            SendAnnouncement = false,
            RemindOneWeekBefore = true,
            RemindOneDayBefore = false
            };
        static readonly DateTime CurrentDateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

    // Observing session with 1-day reminder should queue a reminder 1-day before the start date/time
    [Subject(typeof(ObservingSessionLogic), "reminders")]
    internal class when_creating_a_session_with_a_1_day_reminder : with_event_logic_context
        {
        Establish context = () => EventContext = EventContextBuilder
            .WithCurrentDateTime(CurrentDateTime)
            .Build();
        Because of = () => SessionManager.CreateAsync(model).Wait();
        It should_queue_a_reminder_for_one_day_before_the_event = () =>
            Reminders.Single().ProcessAfter.ShouldEqual(model.StartsAt - TimeSpan.FromDays(1));
        static CreateObservingSessionViewModel model = new CreateObservingSessionViewModel
            {
            Description = "Unit test",
            Venue = "nowhere",
            Title = "Unite Test Event",
            StartsAt = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            SendAnnouncement = false,
            RemindOneWeekBefore = false,
            RemindOneDayBefore = true
            };
        static readonly DateTime CurrentDateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

    // start date closer than 1 week; week reminder should not be queued
    [Subject(typeof(ObservingSessionLogic), "reminders")]
    internal class when_creating_a_session_with_a_1_week_reminder_that_starts_in_less_than_a_week
        : with_event_logic_context
        {
        Establish context = () => EventContext = EventContextBuilder
            .WithCurrentDateTime(CurrentDateTime)
            .Build();
        Because of = () => SessionManager.CreateAsync(model).Wait();
        It should_not_queue_any_reminders = () => Reminders.ShouldBeEmpty();
        static CreateObservingSessionViewModel model = new CreateObservingSessionViewModel
            {
            Description = "Unit test",
            Venue = "nowhere",
            Title = "Unite Test Event",
            StartsAt = CurrentDateTime + TimeSpan.FromDays(7) - TimeSpan.FromTicks(1),
            SendAnnouncement = false,
            RemindOneWeekBefore = true,
            RemindOneDayBefore = false
            };
        static readonly DateTime CurrentDateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

    // start date closer than 24 hours; day reminder should not be queued
    // start date closer than 24 hours; immediate reminder should be queued
    [Subject(typeof(ObservingSessionLogic), "reminders")]
    internal class when_creating_a_session_with_a_1_day_reminder_that_starts_in_less_than_a_day
        : with_event_logic_context
        {
        Establish context = () => EventContext = EventContextBuilder
            .WithCurrentDateTime(CurrentDateTime)
            .Build();
        Because of = () => SessionManager.CreateAsync(model).Wait();
        It should_queue_only_an_immediate_announcement = () => Reminders.Single().ProcessAfter.ShouldEqual(CurrentDateTime);
        static CreateObservingSessionViewModel model = new CreateObservingSessionViewModel
            {
            Description = "Unit test",
            Venue = "nowhere",
            Title = "Unite Test Event",
            StartsAt = CurrentDateTime + TimeSpan.FromDays(1) - TimeSpan.FromTicks(1),
            SendAnnouncement = true,
            RemindOneWeekBefore = true,
            RemindOneDayBefore = true
            };
        static readonly DateTime CurrentDateTime = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        }

}