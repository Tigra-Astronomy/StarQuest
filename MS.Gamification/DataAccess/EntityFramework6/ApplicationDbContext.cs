// This file is part of the MS.Gamification project
// 
// File: ApplicationDbContext.cs  Created: 2016-11-01@19:37
// Last modified: 2016-12-13@03:51

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
        ///     Initializes a new instance of the <see cref="ApplicationDbContext" /> class. This constructor allows for
        ///     use with the EFFORT in-memory provider during unit testing.
        /// </summary>
        /// <param name="connection">A configured database connection.</param>
        public ApplicationDbContext(DbConnection connection) : base(connection, true) {}

        public virtual IDbSet<Challenge> Challenges { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<MissionLevel> MissionLevels { get; set; }

        public virtual IDbSet<MissionTrack> MissionTracks { get; set; }

        public virtual IDbSet<Observation> Observations { get; set; }

        public virtual IDbSet<Mission> Missions { get; set; }

        public virtual IDbSet<Badge> Badges { get; set; }

        public static ApplicationDbContext Create()
            {
            return new ApplicationDbContext();
            }
        }
    }