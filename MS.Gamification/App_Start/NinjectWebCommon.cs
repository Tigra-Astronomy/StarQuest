// This file is part of the MS.Gamification project
// 
// File: NinjectWebCommon.cs  Created: 2016-11-01@19:37
// Last modified: 2016-12-31@10:18

using System;
using System.Configuration;
using System.Data.Entity;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using MS.Gamification;
using MS.Gamification.App_Start;
using MS.Gamification.BusinessLogic.Gamification;
using MS.Gamification.DataAccess;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;
using Ninject;
using Ninject.Web.Common;
using NLog;
using Owin;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace MS.Gamification
    {
    public static class NinjectWebCommon
        {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        internal static IDataProtectionProvider DataProtectionProvider { get; set; }

        public static IKernel NinjectKernel { get; set; }

        public static string RazorEngineTemplatePath { get; private set; }

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
            {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            }

        public static void ConfigureServices(IAppBuilder owinApp)
            {
            DataProtectionProvider = owinApp.GetDataProtectionProvider();
            bootstrapper.Initialize(CreateKernel);
            }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop()
            {
            bootstrapper.ShutDown();
            }

        /// <summary>
        ///     Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
            {
            var kernel = new StandardKernel();
            try
                {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                // Register Ninject as the resolver for WebAPI controllers
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
                NinjectKernel = kernel;
                return kernel;
                }
            catch
                {
                kernel.Dispose();
                throw;
                }
            }

        /// <summary>
        ///     Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
            {
            kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();
            kernel.Bind<DbContext>().To<ApplicationDbContext>().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<EntityFramework6UnitOfWork>().InRequestScope();
            kernel.Bind<IUserStore<ApplicationUser>>().To<ApplicationUserStore>().InRequestScope();
            kernel.Bind<IRoleStore<IdentityRole, string>>()
                .To<RoleStore<IdentityRole, string, IdentityUserRole>>()
                .InRequestScope();
            kernel.Bind<IDataProtectionProvider>()
                .ToMethod(m => DataProtectionProvider)
                .InSingletonScope();
            kernel.Bind<ApplicationUserManager>().ToSelf()
                .InRequestScope();
            kernel.Bind<ApplicationSignInManager>().ToSelf().InRequestScope();
            kernel.Bind<IAuthenticationManager>()
                .ToMethod(m => HttpContext.Current.GetOwinContext().Authentication)
                .InRequestScope();
            kernel.Bind<IIdentity>().ToMethod(p => HttpContext.Current.User.Identity).InRequestScope();
            kernel.Bind<ICurrentUser>().To<AspNetIdentityCurrentUser>();
            kernel.Bind<HttpServerUtilityBase>()
                .ToMethod(c => new HttpServerUtilityWrapper(HttpContext.Current.Server))
                .InRequestScope();
            kernel.Bind<IFileSystemService>().To<WindowsFileSystem>().InSingletonScope();
            kernel.Bind<IImageStore>()
                .To<WebServerImageStore>()
                .InSingletonScope()
                .Named("ValidationImageStore")
                .WithConstructorArgument("rootUrl", ConfigurationManager.AppSettings["validationImagesRootPath"]);
            kernel.Bind<IImageStore>()
                .To<WebServerImageStore>()
                .InSingletonScope()
                .Named("BadgeImageStore")
                .WithConstructorArgument("rootUrl", ConfigurationManager.AppSettings["badgeImagesRootPath"]);
            kernel.Bind<IImageStore>()
                .To<WebServerImageStore>()
                .InSingletonScope()
                .Named("StaticImageStore")
                .WithConstructorArgument("rootUrl", "/Images");
            kernel.Bind<UrlHelper>()
                .ToMethod(m => new UrlHelper(HttpContext.Current.Request.RequestContext))
                .InRequestScope();
            kernel.Bind<IGameEngineService>().To<GameRulesService>().InRequestScope();
            kernel.Bind<IGameNotificationService>().To<GameNotificationService>().InRequestScope();
            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<ViewModelMappingProfile>(); });
            kernel.Bind<IMapper>().ToMethod(m => mapperConfiguration.CreateMapper()).InSingletonScope();
            ResolveRazorEngineTemplatePath(kernel);
            kernel.Bind<IRazorEngineService>()
                .ToMethod(ctx => CreateRazorEngineService(RazorEngineTemplatePath))
                .InSingletonScope();
            kernel.Bind<ILogger>()
                .ToMethod(context => LogManager.GetLogger(context.Request.Target.Name))
                .InRequestScope();
            }

        private static void ResolveRazorEngineTemplatePath(IKernel kernel)
            {
            var util = kernel.Get<HttpServerUtilityBase>();
            RazorEngineTemplatePath = util.MapPath("~/EmailTemplates");
            }

        internal static IRazorEngineService CreateRazorEngineService(string templatePath)
            {
            var config = new TemplateServiceConfiguration
                {
                Language = Language.CSharp,
                TemplateManager = new ResolvePathCheckModifiedTimeTemplateManager(new[] {templatePath})
                };
            var service = RazorEngineService.Create(config);
            return service;
            }
        }
    }