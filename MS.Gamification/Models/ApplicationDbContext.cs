using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MS.Gamification.Models
    {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
            {
            }

        public virtual DbSet<Challenge> Challenges { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        public static ApplicationDbContext Create()
            {
            return new ApplicationDbContext();
            }
        }
    }