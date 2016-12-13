using System.Web.Http;
using FluentScheduler;
using MS.Gamification.GameLogic.ScheduledTasks;
using Ninject;

namespace MS.Gamification
    {
    public partial class Startup
        {
        public void ConfigureScheduler()
            {
            JobManager.JobFactory = new NinjectScheduledTaskFactory(NinjectWebCommon.NinjectKernel);
            JobManager.Initialize(new StarQuestScheduledJobs());
            }

        }
    }