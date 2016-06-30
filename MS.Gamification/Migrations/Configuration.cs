// This file is part of the MS.Gamification project
// 
// File: Configuration.cs  Created: 2016-05-10@22:28
// Last modified: 2016-06-30@23:02

using System.Data.Entity.Migrations;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;

namespace MS.Gamification.Migrations
    {
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
        {
        public Configuration()
            {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MS.Gamification.Models.ApplicationDbContext";
            }

        protected override void Seed(ApplicationDbContext context)
            {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            SeedData.CreateAdministratorAccount(context);
            SeedData.CreateCategories(context);
            context.SaveChanges();
            SeedData.CreateBetaMission(context);
            context.SaveChanges();
            }
        }
    }