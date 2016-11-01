using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MS.Gamification.GameLogic;
using MS.Gamification.Models;

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
        }
    }
