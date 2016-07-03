// This file is part of the MS.Gamification project
// 
// File: NinjectWebCommon.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-03@23:14

using System;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using MS.Gamification.App_Start;
using MS.Gamification.DataAccess;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.HtmlHelpers;
using MS.Gamification.Models;
using Ninject;
using Ninject.Web.Common;
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
            kernel.Bind(typeof(ApplicationDbContext)).ToSelf().InRequestScope();
            kernel.Bind(typeof(IUserStore<ApplicationUser>)).To(typeof(UserStore<ApplicationUser>)).InRequestScope();
            kernel.Bind(typeof(UserManager<ApplicationUser>)).ToSelf().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<EntityFramework6UnitOfWork>().InRequestScope();
            kernel.Bind<HttpServerUtilityBase>().ToMethod(c => new HttpServerUtilityWrapper(HttpContext.Current.Server));
            kernel.Bind<IImageStore>().To<WebServerImageStore>().InSingletonScope();
            kernel.Bind<IIdentity>().ToMethod(p => HttpContext.Current.User.Identity);
            kernel.Bind<ICurrentUser>().To<AspNetIdentityCurrentUser>();
            }
        }
    }