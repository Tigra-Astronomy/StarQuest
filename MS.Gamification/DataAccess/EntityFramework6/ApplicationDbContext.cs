// This file is part of the MS.Gamification project
// 
// File: ApplicationDbContext.cs  Created: 2016-05-10@22:28
// Last modified: 2016-05-22@05:07

using System.Data.Common;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.Models;

namespace MS.Gamification.DataAccess.EntityFramework6
    {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
        public ApplicationDbContext()
            : base("DefaultConnection", false) {}

        public ApplicationDbContext(string connectionString) : base(connectionString) {}

        /// <summary>
        ///     Initializes a new instance of the <see cref="ApplicationDbContext" /> class. This constructor allows for use
        ///     with the EFFORT in-memory provider during unit testing.
        /// </summary>
        /// <param name="connection">A configured database connection.</param>
        public ApplicationDbContext(DbConnection connection) : base(connection, true) {}

        public virtual DbSet<Challenge> Challenges { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<MissionLevel> MissionLevels { get; set; }

        public virtual DbSet<MissionTrack> MissionTracks { get; set; }

        public virtual DbSet<Observation> Observations { get; set; }
        public virtual DbSet<Mission> Missions { get; set; }


        public static ApplicationDbContext Create()
            {
            return new ApplicationDbContext();
            }
        }
    }