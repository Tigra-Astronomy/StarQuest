// This file is part of the MS.Gamification project
// 
// File: ScheduledTaskFactory.cs  Created: 2016-12-12@17:35
// Last modified: 2016-12-31@09:48

using FluentScheduler;
using Ninject;
using NLog;

namespace MS.Gamification.BusinessLogic.Gamification.ScheduledTasks
    {
    /// <summary>
    ///     A factory class for creating scheduled job instances using the Ninject DI kernel.
    /// </summary>
    /// <seealso cref="FluentScheduler.IJobFactory" />
    public class NinjectScheduledTaskFactory : IJobFactory
        {
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();
        private readonly IKernel services;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NinjectScheduledTaskFactory" /> class.
        /// </summary>
        /// <param name="kernel"></param>
        public NinjectScheduledTaskFactory(IKernel kernel)
            {
            services = kernel;
            }

        public IJob GetJobInstance<TJob>() where TJob : IJob
            {
            Log.Debug($"Creating scheduled task {typeof(TJob)} using Ninject");
            var instance = services.Get<TJob>();
            return instance;
            }
        }
    }