// This file is part of the MS.Gamification project
// 
// File: 201607220045476_BadgesAndPreconditions.cs  Created: 2016-07-22@01:45
// Last modified: 2016-07-22@05:47

using System.Data.Entity.Migrations;

namespace MS.Gamification.Migrations
    {
    public partial class BadgesAndPreconditions : DbMigration
        {
        public override void Up()
            {
            CreateTable(
                "dbo.Badges",
                c => new
                    {
                    Id = c.Int(false, true),
                    ImageIdentifier = c.String(),
                    Name = c.String()
                    })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ApplicationUserBadges",
                c => new
                    {
                    ApplicationUser_Id = c.String(false, 128),
                    Badge_Id = c.Int(false)
                    })
                .PrimaryKey(t => new {t.ApplicationUser_Id, t.Badge_Id})
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, true)
                .ForeignKey("dbo.Badges", t => t.Badge_Id, true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Badge_Id);

            AddColumn("dbo.MissionTracks", "BadgeId", c => c.Int());
            AddColumn("dbo.MissionLevels", "Precondition", c => c.String());
            AddColumn("dbo.Missions", "Precondition", c => c.String());
            AddColumn("dbo.AspNetUsers", "Provisioned", c => c.DateTime(false, defaultValueSql: "GETDATE()"));
            CreateIndex("dbo.MissionTracks", "BadgeId");
            AddForeignKey("dbo.MissionTracks", "BadgeId", "dbo.Badges", "Id");
            }

        public override void Down()
            {
            DropForeignKey("dbo.MissionTracks", "BadgeId", "dbo.Badges");
            DropForeignKey("dbo.ApplicationUserBadges", "Badge_Id", "dbo.Badges");
            DropForeignKey("dbo.ApplicationUserBadges", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserBadges", new[] {"Badge_Id"});
            DropIndex("dbo.ApplicationUserBadges", new[] {"ApplicationUser_Id"});
            DropIndex("dbo.MissionTracks", new[] {"BadgeId"});
            DropColumn("dbo.AspNetUsers", "Provisioned");
            DropColumn("dbo.Missions", "Precondition");
            DropColumn("dbo.MissionLevels", "Precondition");
            DropColumn("dbo.MissionTracks", "BadgeId");
            DropTable("dbo.ApplicationUserBadges");
            DropTable("dbo.Badges");
            }
        }
    }