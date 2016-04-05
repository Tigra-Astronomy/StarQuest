// This file is part of the MS.Gamification project
// 
// File: Configuration.cs  Created: 2016-03-23@23:23
// Last modified: 2016-03-24@00:37 by Fern

using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.Controllers;
using MS.Gamification.DataAccess.EntityFramework6;
using MS.Gamification.Models;

namespace MS.Gamification.Migrations
    {
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
        {
        const string AdministratorUserName = "Administrator";
        const string AdministratorDefaultPassword = "password";

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

            CreateAdministratorAccount(context);
            CreateCategories(context);
            context.SaveChanges();
            CreateBetaMission(context);
            context.SaveChanges();
            }

        void CreateBetaMission(ApplicationDbContext context)
            {
            var phaseCategory = context.Categories.Single(p => p.Name == "Phase");
            var planetCategory = context.Categories.Single(p => p.Name == "Planet");
            var openClusterCategory = context.Categories.Single(p => p.Name == "Open Cluster");
            var galaxyCategory = context.Categories.Single(p => p.Name == "Galaxy");
            context.Missions.AddOrUpdate(p => p.Name,
                new Mission
                    {
                    Name = "Beta Mission",
                    AwardTitle = "Beta Tester",
                    Level = 1,
                    Tracks = new List<MissionTrack>
                        {
                        new MissionTrack
                            {
                            Name = "Lunar Track",
                            AwardTitle = "Beta Lunar Observer",
                            Number=1,
                            Challenges= new List<Challenge>
                                {
                                new Challenge
                                    {
                                    Name = "See the waxing crescent moon",
                                    Points = 1,
                                    BookSection = "Moon",
                                    CategoryId = phaseCategory.Id,
                                    Location = "Moon"
                                    },
                                new Challenge
                                    {
                                    Name = "See the full moon",
                                    Points = 1,
                                    BookSection = "Moon",
                                    CategoryId = phaseCategory.Id,
                                    Location = "Moon"
                                    }
                                }
                            },
                        new MissionTrack
                            {
                            Name = "Planetary Track",
                            AwardTitle = "Beta Planetologist",
                            Number=2,
                            Challenges= new List<Challenge>
                                {
                                new Challenge
                                    {
                                    Name = "See Jupiter",
                                    Points = 1,
                                    BookSection = "Solar System",
                                    CategoryId = planetCategory.Id,
                                    Location = "Solar System"
                                    },
                                new Challenge
                                    {
                                    Name = "See Saturn",
                                    Points = 1,
                                    BookSection = "Solar System",
                                    CategoryId = planetCategory.Id,
                                    Location = "Solar System"
                                    }
                                }
                            },
                        new MissionTrack
                            {
                            Name = "Deep Space Track",
                            AwardTitle = "Beta Deep Space Explorer",
                            Number=3,
                            Challenges= new List<Challenge>
                                {
                                new Challenge
                                    {
                                    Name = "See M45, The Pleiades",
                                    Points = 1,
                                    BookSection = "Winter",
                                    CategoryId = openClusterCategory.Id,
                                    Location = "Taurus"
                                    },
                                new Challenge
                                    {
                                    Name = "See M31, The Andromeda Galaxy",
                                    Points = 1,
                                    BookSection = "Autumn",
                                    CategoryId = galaxyCategory.Id,
                                    Location = "Andromeda"
                                    }
                                }
                            }
                        }
                    }
                );
            }

        void CreateCategories(ApplicationDbContext context)
            {
            foreach (var seed in SeedData.Categories)
                {
                context.Categories.AddOrUpdate(p => p.Name,
                    new Category() {Name = seed});
                }
            }

        static void CreateAdministratorAccount(ApplicationDbContext context)
            {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            if (!context.Roles.Any(role => role.Name == AdminController.AdministratorRoleName))
                roleManager.Create(new IdentityRole {Name = AdminController.AdministratorRoleName});
            if (!context.Roles.Any(role => role.Name == AdminController.ModeratorRoleName))
                roleManager.Create(new IdentityRole {Name = AdminController.ModeratorRoleName});
            if (!context.Users.Any(user => user.UserName == AdministratorUserName))
                {
                var user = new ApplicationUser {UserName = AdministratorUserName};
                userManager.Create(user, AdministratorDefaultPassword);
                userManager.AddToRole(user.Id, AdminController.AdministratorRoleName);
                }
            }
        }
    }
