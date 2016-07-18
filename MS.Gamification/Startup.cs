// This file is part of the MS.Gamification project
// 
// File: Startup.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-17@21:45

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
            }
        }
    }