// This file is part of the MS.Gamification project
// 
// File: SeedData.cs  Created: 2016-05-10@22:28
// Last modified: 2016-06-30@23:19

using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.Gamification.Controllers;
using MS.Gamification.DataAccess.EntityFramework6;

namespace MS.Gamification.Models
    {
    public static class SeedData
        {
        internal const string AdministratorUserName = "Administrator";
        internal const string AdministratorDefaultPassword = "password";
        public static List<string> Categories = new List<string>
            {
            "Artificial Satellite",
            "Asterism",
            "Astrometry",
            "Comet",
            "Constellation",
            "Crater",
            "Dark Nebula",
            "Diffuse Nebula",
            "Double Star",
            "Eclipse",
            "Emission Nebula",
            "Galaxy",
            "Globular Cluster",
            "Mare",
            "Meteor",
            "Minor Planet",
            "Open Cluster",
            "Phase",
            "Phenomenon",
            "Planet",
            "Planetary Nebula",
            "Region",
            "Satellite",
            "Sky",
            "Star",
            "Surface Feature",
            "Variable Star"
            };

        internal static void CreateCategories(ApplicationDbContext context)
            {
            foreach (var seed in Categories)
                {
                context.Categories.AddOrUpdate(p => p.Name,
                    new Category {Name = seed});
                }
            }

        internal static void CreateAdministratorAccount(ApplicationDbContext context)
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
                //ToDo: Hard coded secrets!! These need to come from web.config or similar
                var user = new ApplicationUser
                        {UserName = AdministratorUserName, Email = "nobody@nowhere.com", EmailConfirmed = true};
                userManager.Create(user, AdministratorDefaultPassword);
                userManager.AddToRole(user.Id, AdminController.AdministratorRoleName);
                }
            }

        internal static void CreateBetaMission(ApplicationDbContext context)
            {
            var phaseCategory = context.Categories.Single(p => p.Name == "Phase");
            var planetCategory = context.Categories.Single(p => p.Name == "Planet");
            var openClusterCategory = context.Categories.Single(p => p.Name == "Open Cluster");
            var galaxyCategory = context.Categories.Single(p => p.Name == "Galaxy");
            context.Missions.AddOrUpdate(p => p.Name,
                new MissionLevel
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
                            Number = 1,
                            Challenges = new List<Challenge>
                                {
                                new Challenge
                                    {
                                    Name = "See the waxing crescent moon",
                                    Points = 1,
                                    BookSection = "Moon",
                                    CategoryId = phaseCategory.Id,
                                    Location = "Moon",
                                    ValidationImage = "Moon Waxing Crescent.gif"
                                    },
                                new Challenge
                                    {
                                    Name = "See the full moon",
                                    Points = 1,
                                    BookSection = "Moon",
                                    CategoryId = phaseCategory.Id,
                                    Location = "Moon",
                                    ValidationImage = "Moon Full.gif"
                                    }
                                }
                            },
                        new MissionTrack
                            {
                            Name = "Planetary Track",
                            AwardTitle = "Beta Planetologist",
                            Number = 2,
                            Challenges = new List<Challenge>
                                {
                                new Challenge
                                    {
                                    Name = "See Jupiter",
                                    Points = 1,
                                    BookSection = "Solar System",
                                    CategoryId = planetCategory.Id,
                                    Location = "Solar System",
                                    ValidationImage = "Jupiter.png"
                                    },
                                new Challenge
                                    {
                                    Name = "See Saturn",
                                    Points = 1,
                                    BookSection = "Solar System",
                                    CategoryId = planetCategory.Id,
                                    Location = "Solar System",
                                    ValidationImage = "Saturn.png"
                                    }
                                }
                            },
                        new MissionTrack
                            {
                            Name = "Deep Space Track",
                            AwardTitle = "Beta Deep Space Explorer",
                            Number = 3,
                            Challenges = new List<Challenge>
                                {
                                new Challenge
                                    {
                                    Name = "See M45, The Pleiades",
                                    Points = 1,
                                    BookSection = "Winter",
                                    CategoryId = openClusterCategory.Id,
                                    Location = "Taurus",
                                    ValidationImage = "M45-Pleiades.png"
                                    },
                                new Challenge
                                    {
                                    Name = "See M31, The Andromeda Galaxy",
                                    Points = 1,
                                    BookSection = "Autumn",
                                    CategoryId = galaxyCategory.Id,
                                    Location = "Andromeda",
                                    ValidationImage = "M31.png"
                                    }
                                }
                            }
                        }
                    }
            );
            }
        }
    }