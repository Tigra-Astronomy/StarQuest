// This file is part of the MS.Gamification project
// 
// File: ConfigureScheduler.cs  Created: 2016-12-12@18:14
// Last modified: 2016-12-31@12:58

using System.Data.Entity;
using AutoMapper;
using FluentScheduler;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.DataProtection;
using MS.Gamification.App_Start;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.BusinessLogic.Gamification.ScheduledTasks;
using MS.Gamification.DataAccess;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;
using Ninject;
using RazorEngine.Templating;

namespace MS.Gamification
    {
    public partial class Startup
        {
        public void ConfigureScheduler()
            {
            var schedulerKernel = CreateSchedulerCompositionRoot();
            JobManager.JobFactory = new NinjectScheduledTaskFactory(schedulerKernel);
            JobManager.Initialize(new StarQuestScheduledJobs());
            }

        /// <summary>
        ///     Creates a DI container for the scheduler service. Tasks execute outside of any web
        ///     request, so we can't use any services that rely on a HttpContext. Similarly,  we
        ///     cannot use InRequestScope() so we must use InTransientScope() or InsingletonScope()
        ///     instead.
        /// </summary>
        private static IKernel CreateSchedulerCompositionRoot()
            {
            //ToDo: Create a .InScheduledTaskScope() extension for Ninject
            var kernel = new StandardKernel();
            kernel.Bind<ApplicationDbContext>().ToSelf().InTransientScope();
            kernel.Bind<DbContext>().To<ApplicationDbContext>().InTransientScope();
            kernel.Bind<IUnitOfWork>().To<EntityFramework6UnitOfWork>().InTransientScope();
            kernel.Bind<IUserStore<ApplicationUser>>().To<ApplicationUserStore>().InTransientScope();
            kernel.Bind<IRoleStore<IdentityRole, string>>()
                .To<RoleStore<IdentityRole, string, IdentityUserRole>>()
                .InTransientScope();
            kernel.Bind<IDataProtectionProvider>()
                .ToMethod(m => NinjectWebCommon.DataProtectionProvider)
                .InSingletonScope();
            kernel.Bind<ApplicationUserManager>().ToSelf()
                .InTransientScope();
            kernel.Bind<ApplicationSignInManager>().ToSelf().InTransientScope();
            kernel.Bind<IFileSystemService>().To<WindowsFileSystem>().InSingletonScope();
            kernel.Bind<IGameEngineService>().To<GameRulesService>().InTransientScope();
            kernel.Bind<IGameNotificationService>().To<GameNotificationService>().InTransientScope();
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<ViewModelMappingProfile>(); });
            kernel.Bind<IMapper>().ToMethod(m => mapperConfiguration.CreateMapper()).InSingletonScope();
            kernel.Bind<IRazorEngineService>()
                .ToMethod(ctx => NinjectWebCommon.CreateRazorEngineService(NinjectWebCommon.RazorEngineTemplatePath))
                .InSingletonScope();
            return kernel;
            //ToDo: create a UrlHelper implementation that can be used outside of any HttpContext.
            }
        }
    }