// This file is part of the MS.Gamification project
// 
// File: StarQuestScheduledJobs.cs  Created: 2016-12-12@18:35
// Last modified: 2016-12-31@13:38

using System;
using System.Configuration;
using FluentScheduler;

namespace MS.Gamification.BusinessLogic.Gamification.ScheduledTasks
    {
    public class StarQuestScheduledJobs : Registry
        {
        public StarQuestScheduledJobs()
            {
            var moderatorDailySummarySetting = ConfigurationManager.AppSettings["DailyModeratorNotificationTime"];
            var moderatorDailySummary = TimeSpan.Parse(moderatorDailySummarySetting);
            Schedule<ModeratorDailySummaryTask>()
                .ToRunEvery(1)
                .Days()
                .At(moderatorDailySummary.Hours, moderatorDailySummary.Minutes);
            }
        }
    }