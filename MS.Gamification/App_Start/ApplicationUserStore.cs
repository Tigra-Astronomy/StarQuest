using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;

namespace MS.Gamification.App_Start
    {
    class ApplicationUserStore : UserStore<ApplicationUser>
        {
        public ApplicationUserStore(ApplicationDbContext context) : base(context) {}
        }
    }