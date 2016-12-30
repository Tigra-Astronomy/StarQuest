// This file is part of the MS.Gamification project
// 
// File: ModeratorDailySummaryTaskSpecs.cs  Created: 2016-12-12@19:45
// Last modified: 2016-12-13@04:11

using System.Collections.Generic;
using FakeItEasy;
using Machine.Specifications;
using MS.Gamification.GameLogic;
using MS.Gamification.GameLogic.ScheduledTasks;
using MS.Gamification.Models;
using MS.Gamification.Tests.TestHelpers;
using MS.Gamification.ViewModels.Moderation;

namespace MS.Gamification.Tests.ScheduledJobs
    {
    [Subject(typeof(ModeratorDailySummaryTask), "No Work")]
    class when_the_task_runs_and_there_are_no_pending_observations : with_job_context<ModeratorDailySummaryTask>
        {
        Establish context = () =>
            {
            Job = JobContextBuilder
                .NotifyWith(A.Fake<IGameNotificationService>())
                .Build();
            };

        Because of = () => Job.Execute();

        It should_not_send_any_notifications = () =>
            A.CallTo(() => JobContextBuilder.Notifier.PendingObservationSummary(
                    A<ApplicationUser>.Ignored,
                    A<IEnumerable<ModerationQueueItem>>.Ignored))
                .MustNotHaveHappened();
        }

    [Subject(typeof(ModeratorDailySummaryTask), "Single moderator Single observation")]
    class when_the_task_runs_and_there_is_a_single_pending_observations : with_job_context<ModeratorDailySummaryTask>
        {
        Establish context = () =>
            {
            Job = JobContextBuilder
                .NotifyWith(A.Fake<IGameNotificationService>())
                .WithData(d => d
                        .WithModerator("mod", "Joe Moderator")
                        .WithStandardUser("user", "Joe User")
                        .WithObservation().WithId(2).AwaitingModeration().ForUserId("user").ForChallenge(100).BuildObservation()
                )
                .Build();
            };
        Because of = () => Job.Execute();
        It should_send_one_notification = () =>
            A.CallTo(() => JobContextBuilder.Notifier.PendingObservationSummary(
                    A<ApplicationUser>.That.Matches(p => p.Id == "mod"),
                    A<IEnumerable<ModerationQueueItem>>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    [Subject(typeof(ModeratorDailySummaryTask), "Two moderators and two observations")]
    class when_the_task_runs_and_there_are_two_pending_observations_and_two_moderators : with_job_context<ModeratorDailySummaryTask>
        {
        Establish context = () =>
            {
            Job = JobContextBuilder
                .NotifyWith(A.Fake<IGameNotificationService>())
                .WithData(d => d
                        .WithModerator("mod1", "Joe Moderator")
                        .WithModerator("mod2", "Jim Moderator")
                        .WithStandardUser("user", "Joe User")
                        .WithObservation().WithId(2).AwaitingModeration().ForUserId("user").ForChallenge(100).BuildObservation()
                        .WithObservation().WithId(3).AwaitingModeration().ForUserId("user").ForChallenge(100).BuildObservation()
                )
                .Build();
            };
        Because of = () => Job.Execute();
        It should_send_one_notification_to_joe = () =>
            A.CallTo(() => JobContextBuilder.Notifier.PendingObservationSummary(
                    A<ApplicationUser>.That.Matches(p => p.Id == "mod1"),
                    A<IEnumerable<ModerationQueueItem>>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        It should_send_one_notification_to_jim = () =>
            A.CallTo(() => JobContextBuilder.Notifier.PendingObservationSummary(
                    A<ApplicationUser>.That.Matches(p => p.Id == "mod2"),
                    A<IEnumerable<ModerationQueueItem>>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        It should_send_a_total_of_two_notifications = () =>
            A.CallTo(() => JobContextBuilder.Notifier.PendingObservationSummary(
                    A<ApplicationUser>.Ignored,
                    A<IEnumerable<ModerationQueueItem>>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Twice);
        }
    }