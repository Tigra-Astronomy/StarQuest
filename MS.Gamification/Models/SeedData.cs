// This file is part of the MS.Gamification project
// 
// File: SeedData.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-22@11:39

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
            if (!context.Roles.Any(role => role.Name == RequiresAdministratorRights.AdministratorRoleName))
                roleManager.Create(new IdentityRole {Name = RequiresAdministratorRights.AdministratorRoleName});
            if (!context.Roles.Any(role => role.Name == RequiresAdministratorRights.ModeratorRoleName))
                roleManager.Create(new IdentityRole {Name = RequiresAdministratorRights.ModeratorRoleName});
            if (!context.Users.Any(user => user.UserName == AdministratorUserName))
                {
                //ToDo: Hard coded secrets!! These need to come from web.config or similar
                var user = new ApplicationUser
                    {UserName = AdministratorUserName, Email = "nobody@nowhere.com", EmailConfirmed = true};
                userManager.Create(user, AdministratorDefaultPassword);
                userManager.AddToRole(user.Id, RequiresAdministratorRights.AdministratorRoleName);
                }
            }

        internal static void CreateBetaMission(ApplicationDbContext context)
            {
            var phaseCategory = context.Categories.Single(p => p.Name == "Phase");
            var planetCategory = context.Categories.Single(p => p.Name == "Planet");
            var openClusterCategory = context.Categories.Single(p => p.Name == "Open Cluster");
            var galaxyCategory = context.Categories.Single(p => p.Name == "Galaxy");
            var missionTitle = "Beta Test Mission";
            context.Missions.AddOrUpdate(p => p.Title,
                new Mission
                    {
                    Title = missionTitle,
                    MissionLevels = new List<MissionLevel>
                        {
                        new MissionLevel
                            {
                            Name = "Alpha Mission",
                            AwardTitle = "Legendary Alpha Tester",
                            Level = 1,
                            Tracks = new List<MissionTrack>
                                {
                                new MissionTrack
                                    {
                                    Name = "Lunar Track",
                                    AwardTitle = "Alpha Lunar Observer",
                                    Badge =
                                    new Badge {Name = "Alpha Lunar Observer", ImageIdentifier = "alpha-lunar-observer-1"},
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
                                            ValidationImage = "Moon-Waxing-Crescent"
                                            },
                                        new Challenge
                                            {
                                            Name = "See the full moon",
                                            Points = 1,
                                            BookSection = "Moon",
                                            CategoryId = phaseCategory.Id,
                                            Location = "Moon",
                                            ValidationImage = "Moon-Full"
                                            }
                                        }
                                    },
                                new MissionTrack
                                    {
                                    Name = "Planetary Track",
                                    AwardTitle = "Alpha Planetologist",
                                    Number = 2,
                                    Badge = new Badge {Name = "Alpha Planetologist", ImageIdentifier = "alpha-planetologist-1"},
                                    Challenges = new List<Challenge>
                                        {
                                        new Challenge
                                            {
                                            Name = "See Jupiter",
                                            Points = 1,
                                            BookSection = "Solar System",
                                            CategoryId = planetCategory.Id,
                                            Location = "Solar System",
                                            ValidationImage = "Jupiter"
                                            },
                                        new Challenge
                                            {
                                            Name = "See Saturn",
                                            Points = 1,
                                            BookSection = "Solar System",
                                            CategoryId = planetCategory.Id,
                                            Location = "Solar System",
                                            ValidationImage = "Saturn"
                                            }
                                        }
                                    },
                                new MissionTrack
                                    {
                                    Name = "Deep Space Track",
                                    AwardTitle = "Alpha Deep Space Explorer",
                                    Number = 3,
                                    Badge =
                                    new Badge
                                        {Name = "Alpha Deep Space Explorer", ImageIdentifier = "alpha-deep-space-explorer-1"},
                                    Challenges = new List<Challenge>
                                        {
                                        new Challenge
                                            {
                                            Name = "See M45, The Pleiades",
                                            Points = 1,
                                            BookSection = "Winter",
                                            CategoryId = openClusterCategory.Id,
                                            Location = "Taurus",
                                            ValidationImage = "M45-Pleiades"
                                            },
                                        new Challenge
                                            {
                                            Name = "See M31, The Andromeda Galaxy",
                                            Points = 1,
                                            BookSection = "Autumn",
                                            CategoryId = galaxyCategory.Id,
                                            Location = "Andromeda",
                                            ValidationImage = "M31"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                );
            }
        }
    }