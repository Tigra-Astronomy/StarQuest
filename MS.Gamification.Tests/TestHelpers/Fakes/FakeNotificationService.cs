// This file is part of the MS.Gamification project
// 
// File: FakeNotificationService.cs  Created: 2016-11-01@19:37
// Last modified: 2016-12-13@03:32

using System.Collections.Generic;
using System.Threading.Tasks;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.Models;
using MS.Gamification.ViewModels.Moderation;

namespace MS.Gamification.Tests.TestHelpers.Fakes
    {
    class FakeNotificationService : IGameNotificationService
        {
        public Task ObservationApproved(Observation observation)
            {
            return Task.FromResult(0);
            }

        public Task BadgeAwarded(Badge badge, ApplicationUser user, MissionTrack track)
            {
            return Task.FromResult(0);
            }

        public Task PendingObservationSummary(ApplicationUser user, IEnumerable<ModerationQueueItem> observations)
            {
            return Task.FromResult(0);
            }
        }
    }