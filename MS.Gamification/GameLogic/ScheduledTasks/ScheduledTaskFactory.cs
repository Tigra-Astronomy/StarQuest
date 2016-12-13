using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using FluentScheduler;
using Ninject;
using NLog;

namespace MS.Gamification.GameLogic.ScheduledTasks
    {
    /// <summary>
    /// A factory class for creating scheduled job instances using the Ninject DI kernel.
    /// </summary>
    /// <seealso cref="FluentScheduler.IJobFactory" />
    public class NinjectScheduledTaskFactory : IJobFactory
        {
        private readonly IKernel services;
        private static readonly ILogger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="NinjectScheduledTaskFactory"/> class.
        /// </summary>
        /// <param name="kernel"></param>
        public NinjectScheduledTaskFactory(IKernel kernel)
        {
            this.services = kernel;
            }

        public IJob GetJobInstance<TJob>() where TJob : IJob
            {
            Log.Debug($"Creating scheduled task {typeof(TJob)} using Ninject");
            var instance = services.Get<TJob>();
            return instance;
            }
        }
    }