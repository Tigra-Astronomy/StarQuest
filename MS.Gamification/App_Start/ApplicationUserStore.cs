using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;

namespace MS.Gamification
    {
    class ApplicationUserStore : UserStore<ApplicationUser>
        {
        public ApplicationUserStore(ApplicationDbContext context) : base(context) {}
        }
    }