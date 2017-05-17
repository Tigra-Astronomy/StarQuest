// This file is part of the MS.Gamification project
// 
// File: Configuration.cs  Created: 2016-11-01@19:37
// Last modified: 2017-05-16@20:36

using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
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
            try
                {
                context.EnsureDefaultRolesAndUsers();
                context.EnsureCategories();
                context.SaveChanges();
                // Create sample mission data, but only if there are no missions in the database.
                if (!context.Missions.Any())
                    {
                    SeedData.CreateBetaMission(context);
                    context.SaveChanges();
                    }
                }
            catch (DbEntityValidationException eve)
                {
                Debug.WriteLine("Entity validation errors:");
                foreach (var dbEntityValidationResult in eve.EntityValidationErrors)
                    {
                    Debug.WriteLine(dbEntityValidationResult.Entry.Entity.GetType());
                    foreach (var dbValidationError in dbEntityValidationResult.ValidationErrors)
                        {
                        Debug.WriteLine($"--> {dbValidationError.PropertyName}: {dbValidationError.ErrorMessage}");
                        }
                    }
                throw;
                }
            }
        }
    }