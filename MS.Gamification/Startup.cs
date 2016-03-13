using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MS.Gamification.Startup))]
namespace MS.Gamification
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
