using MS.Gamification.Models;

namespace MS.Gamification.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidationImages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Challenges", "ValidationImage", c => c.String(nullable: false, maxLength: 255, defaultValue: Challenge.NoImagePlaceholder));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Challenges", "ValidationImage");
        }
    }
}
