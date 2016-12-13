// This file is part of the MS.Gamification project
// 
// File: FakeNotificationService.cs  Created: 2016-11-01@19:37
// Last modified: 2016-12-13@00:43

using System.Collections.Generic;
using System.Threading.Tasks;
using MS.Gamification.GameLogic;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

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

        public Task PendingObservationSummary(ApplicationUser user, IEnumerable<ObservationDetailsViewModel> observations)
            {
            return Task.FromResult(0);
            }
        }
    }