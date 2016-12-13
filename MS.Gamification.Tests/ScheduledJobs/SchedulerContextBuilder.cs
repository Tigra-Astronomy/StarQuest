// This file is part of the MS.Gamification project
// 
// File: SchedulerContextBuilder.cs  Created: 2016-12-12@20:25
// Last modified: 2016-12-13@00:37

using System;
using AutoMapper;
using FluentScheduler;
using Machine.Specifications;
using MS.Gamification.App_Start;
using MS.Gamification.DataAccess;
using MS.Gamification.GameLogic;
using MS.Gamification.GameLogic.ScheduledTasks;
using MS.Gamification.Tests.TestHelpers;
using Ninject;

namespace MS.Gamification.Tests.ScheduledJobs
    {
    /// <summary>
    ///     Test context base class for testing MVC Controllers. Use of this class provides a fluent builder for
    ///     initializing a transient in-memory data context and the HTTP request context, and ensures that  everything
    ///     is correctly cleaned up at the end of the test context.
    /// </summary>
    /// <typeparam name="TController">The type of the MVC controller under test.</typeparam>
    /// <remarks>
    ///     The unit under test should be stored in the field <see cref="ControllerUnderTest" />. This should be built
    ///     using the <see cref="ControllerContextBuilder{TController}" /> in <see cref="ContextBuilder" />.
    /// </remarks>
    class ScheduledJobContextBuilder<TJob> where TJob : class, IJob
        {
        readonly DataContextBuilder dataBuilder = new DataContextBuilder();

        public IJobFactory JobFactory { get; private set; }

        IUnitOfWork UnitOfWork { get; set; }

        public IGameNotificationService Notifier { get; private set; }

        public ScheduledJobContextBuilder<TJob> WithData(Action<DataContextBuilder> builderMethod)
            {
            builderMethod(dataBuilder);
            return this;
            }

        public TJob Build()
            {
            UnitOfWork = dataBuilder.Build();
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<ViewModelMappingProfile>(); });
            mapperConfiguration.AssertConfigurationIsValid();
            var kernel = BuildNinjectKernel();
            JobFactory = new NinjectScheduledTaskFactory(kernel);
            var job = JobFactory.GetJobInstance<TJob>();
            if (job == null)
                throw new SpecificationException(
                    $"ScheduledJobContextBuilder: Unable to create instance of type {nameof(TJob)}");
            return job as TJob;
            }

        IKernel BuildNinjectKernel()
            {
            IKernel kernel = new StandardKernel();
            kernel.Bind<IUnitOfWork>().ToMethod(u => UnitOfWork);
            kernel.Bind<IGameNotificationService>().ToMethod(u => Notifier).InTransientScope();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<ViewModelMappingProfile>());
            kernel.Bind<IMapper>().ToMethod(m => mapperConfig.CreateMapper()).InTransientScope();
            return kernel;
            }

        public ScheduledJobContextBuilder<TJob> NotifyWith(IGameNotificationService notifier)
            {
            Notifier = notifier;
            return this;
            }
        }
    }