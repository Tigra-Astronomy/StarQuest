// This file is part of the MS.Gamification project
// 
// File: StarQuestScheduledJobs.cs  Created: 2017-05-17@19:32
// Last modified: 2017-05-19@02:01

using System;
using System.Configuration;
using FluentScheduler;
using MS.Gamification.BusinessLogic.EventManagement;
using MS.Gamification.BusinessLogic.QueueProcessing;

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
            var queuedTaskIntervalSetting = ConfigurationManager.AppSettings["ProcessQueuedTasksEvery"];
            var queuedTaskProcessingInterval = TimeSpan.Parse(queuedTaskIntervalSetting);
            var intervalInSeconds = (int)queuedTaskProcessingInterval.TotalSeconds;
            Schedule<QueueProcessorTask>()
                .ToRunEvery(intervalInSeconds)
                .Seconds();
            }
        }
    }