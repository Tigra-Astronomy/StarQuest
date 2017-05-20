using System;
using MS.Gamification.BusinessLogic.QueueProcessing;

namespace MS.Gamification.Tests.QueueProcessing {
    class UnitTestTimeProvider : ITimeProvider
        {
        public UnitTestTimeProvider()
            {
            UtcNow = DateTime.UtcNow;
            }

        public DateTime UtcNow { get; internal set; }
        }
    }