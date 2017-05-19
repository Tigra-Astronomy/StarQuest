// This file is part of the MS.Gamification project
// 
// File: ITimeProvider.cs  Created: 2017-05-18@22:36
// Last modified: 2017-05-18@22:42

using System;

namespace MS.Gamification.BusinessLogic.QueueProcessing
    {
    public interface ITimeProvider
        {
        DateTime UtcNow { get; }
        }

    internal class SystemClockTimeProvider : ITimeProvider
        {
        public DateTime UtcNow => DateTime.UtcNow;
        }
    }