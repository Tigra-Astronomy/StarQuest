// This file is part of the MS.Gamification project
// 
// File: 201607201222135_BadgesAndPreconditions.cs  Created: 2016-07-20@13:22
// Last modified: 2016-07-20@13:23

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
                    Name = c.String(),
                    ApplicationUser_Id = c.String(maxLength: 128)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);

            AddColumn("dbo.AspNetUsers", "Provisioned", c => c.DateTime(false, defaultValueSql: "GETDATE()"));
            }

        public override void Down()
            {
            DropForeignKey("dbo.Badges", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Badges", new[] {"ApplicationUser_Id"});
            DropColumn("dbo.AspNetUsers", "Provisioned");
            DropTable("dbo.Badges");
            }
        }
    }