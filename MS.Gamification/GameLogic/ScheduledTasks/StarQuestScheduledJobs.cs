using FluentScheduler;

namespace MS.Gamification.GameLogic.ScheduledTasks
    {
    public class StarQuestScheduledJobs : Registry
        {
        public StarQuestScheduledJobs()
            {
            Schedule<ModeratorDailySummaryTask>().ToRunEvery(1).Days().At(9, 0);
            }
        }
    }