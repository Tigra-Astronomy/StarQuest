using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.Models;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
            {
            }

        public ApplicationDbContext(string connectionString) : base(connectionString) {}

        public virtual DbSet<Challenge> Challenges { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Mission> Missions { get; set; }
        public virtual DbSet<MissionTrack> MissionTracks { get; set; }
        public virtual DbSet<Observation> Observations { get; set; }
        

        public static ApplicationDbContext Create()
            {
            return new ApplicationDbContext();
            }
        }
    }