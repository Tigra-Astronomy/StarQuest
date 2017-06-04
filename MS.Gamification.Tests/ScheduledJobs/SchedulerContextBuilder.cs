// This file is part of the MS.Gamification project
// 
// File: SchedulerContextBuilder.cs  Created: 2017-05-16@17:41
// Last modified: 2017-06-04@00:54

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using AutoMapper;
using FluentScheduler;
using JetBrains.Annotations;
using Machine.Specifications;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.App_Start;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.BusinessLogic.Gamification.ScheduledTasks;
using MS.Gamification.BusinessLogic.QueueProcessing;
using MS.Gamification.DataAccess;
using MS.Gamification.Models;
using MS.Gamification.Tests.QueueProcessing;
using MS.Gamification.Tests.TestHelpers;
using Ninject;

namespace MS.Gamification.Tests.ScheduledJobs
    {
    /// <summary>
    ///     Test context base class for testing MVC Controllers. Use of this class provides a fluent
    ///     builder for initializing a transient in-memory data context and the HTTP request
    ///     context, and ensures that  everything is correctly cleaned up at the end of the test
    ///     context.
    /// </summary>
    /// <typeparam name="TController">The type of the MVC controller under test.</typeparam>
    /// <remarks>
    ///     The unit under test should be stored in the field <see cref="ControllerUnderTest" />.
    ///     This should be built using the <see cref="ControllerContextBuilder{TController}" /> in
    ///     <see cref="ContextBuilder" />.
    /// </remarks>
    class ScheduledJobContextBuilder<TJob> where TJob : class, IJob
        {
        [NotNull] readonly JobContext<TJob> context = new JobContext<TJob>();
        [NotNull] readonly DataContextBuilder dataBuilder = new DataContextBuilder();

        [NotNull]
        public IJobFactory JobFactory { get; private set; }

        public UnitTestTimeProvider TimeProvider { get; } = new UnitTestTimeProvider();

        public ScheduledJobContextBuilder<TJob> WithData(Action<DataContextBuilder> builderMethod)
            {
            builderMethod(dataBuilder);
            return this;
            }

        [NotNull]
        public JobContext<TJob> Build()
            {
            context.UnitOfWork = dataBuilder.Build();
            context.DataContext = dataBuilder.DataContext;
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<ViewModelMappingProfile>(); });
            mapperConfiguration.AssertConfigurationIsValid();
            var kernel = BuildNinjectKernel(context);
            JobFactory = new NinjectScheduledTaskFactory(kernel);
            var job = JobFactory.GetJobInstance<TJob>();
            if (job == null)
                throw new SpecificationException(
                    $"ScheduledJobContextBuilder: Unable to create instance of type {nameof(TJob)}");
            context.Job = job as TJob;
            return context;
            }

        IKernel BuildNinjectKernel(JobContext<TJob> jobContext)
            {
            var kernel = jobContext.DependencyResolver;
            kernel.Bind<ITimeProvider>().ToMethod(t => jobContext.TimeProvider).InTransientScope();
            kernel.Bind<DbConnection>().ToMethod(ctx => dataBuilder.DataConnection);
            kernel.Bind<DbContext>().ToMethod(ctx => dataBuilder.DataContext);
            kernel.Bind<IUnitOfWork>().ToMethod(u => jobContext.UnitOfWork);
            kernel.Bind<IGameNotificationService>().ToMethod(u => jobContext.Notifier).InTransientScope();
            kernel.Bind<IUserStore<ApplicationUser>>().To<ApplicationUserStore>().InTransientScope();
            kernel.Bind<IRoleStore<IdentityRole, string>>()
                .To<RoleStore<IdentityRole, string, IdentityUserRole>>()
                .InTransientScope();
            kernel.Bind<RoleManager<IdentityRole>>().ToSelf().InTransientScope();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<ViewModelMappingProfile>());
            kernel.Bind<IMapper>().ToMethod(m => mapperConfig.CreateMapper()).InTransientScope();
            kernel.Bind<IQueueProcessorFactory>()
                .ToMethod(ctx => new NinjectQueueProcessorFactory(kernel, CreateUnitTestWorkItemMappings()))
                .InTransientScope();
            return kernel;
            }

        IDictionary<Type, Type> CreateUnitTestWorkItemMappings() => new Dictionary<Type, Type>
            {
            [typeof(ObservingSessionReminder)] = typeof(UnitTestWorkItemProcessor),
            [typeof(UnitTestWorkItem)] = typeof(UnitTestWorkItemProcessor)
            };

        public ScheduledJobContextBuilder<TJob> NotifyWith(IGameNotificationService notifier)
            {
            context.Notifier = notifier;
            return this;
            }

        public ScheduledJobContextBuilder<TJob> StartTime(DateTime startAt)
            {
            TimeProvider.UtcNow = startAt;
            return this;
            }

        [NotNull]
        public ScheduledJobContextBuilder<TJob> AddDependency([NotNull] Action<IKernel> binder)
            {
            Contract.Requires(binder != null);
            Contract.Ensures(Contract.Result<ScheduledJobContextBuilder<TJob>>() != null);
            binder(context.DependencyResolver);
            return this;
            }
        }
    }