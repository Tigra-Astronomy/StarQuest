// This file is part of the MS.Gamification project
// 
// File: NinjectWebCommon.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-17@08:55

using System;
using System.Configuration;
using System.Data.Entity;
using System.Security.Principal;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using MS.Gamification.App_Start;
using MS.Gamification.DataAccess;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.GameLogic;
using MS.Gamification.HtmlHelpers;
using MS.Gamification.Models;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Common;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace MS.Gamification.App_Start
    {
    public static class NinjectWebCommon
        {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
            {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
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
            kernel.Bind<ApplicationUserManager>()
                .ToMethod(m => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>())
                .InRequestScope();
            kernel.Bind<ApplicationSignInManager>().ToSelf().InRequestScope();
            kernel.Bind<IAuthenticationManager>()
                .ToMethod(m => HttpContext.Current.GetOwinContext().Authentication)
                .InRequestScope();
            kernel.Bind<IDataProtectionProvider>().To
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
            kernel.Bind<IIdentity>().ToMethod(p => HttpContext.Current.User.Identity).InRequestScope();
            kernel.Bind<ICurrentUser>().To<AspNetIdentityCurrentUser>();
            kernel.Bind<GameRulesService>().ToSelf().InRequestScope();

            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<ViewModelMappingProfile>(); });
            kernel.Bind<IMapper>().ToMethod(m => mapperConfiguration.CreateMapper()).InSingletonScope();
            kernel.Bind<IRazorEngineService>().ToMethod(CreateRazorEngineService).InSingletonScope();
            }

        private static IRazorEngineService CreateRazorEngineService(IContext context)
            {
            var util = context.Kernel.Get<HttpServerUtilityBase>();
            var templatePath = util.MapPath("~/EmailTemplates");
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