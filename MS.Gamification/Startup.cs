// This file is part of the MS.Gamification project
// 
// File: Startup.cs  Created: 2016-08-20@23:12
// Last modified: 2016-12-12@19:20

using Microsoft.Owin;
using MS.Gamification;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace MS.Gamification
    {
    public partial class Startup
        {
        public void Configuration(IAppBuilder app)
            {
            NinjectWebCommon.ConfigureServices(app);
            ConfigureAuth(app);
            ConfigureScheduler();
            }
        }
    }