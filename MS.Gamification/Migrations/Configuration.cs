// This file is part of the MS.Gamification project
// 
// File: Configuration.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-22@13:45

using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
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
                SeedData.CreateAdministratorAccount(context);
                SeedData.CreateCategories(context);
                context.SaveChanges();
                SeedData.CreateBetaMission(context);
                context.SaveChanges();
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