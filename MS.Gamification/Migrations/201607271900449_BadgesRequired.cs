namespace MS.Gamification.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BadgesRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MissionTracks", "BadgeId", "dbo.Badges");
            DropIndex("dbo.MissionTracks", new[] { "BadgeId" });
            AlterColumn("dbo.MissionTracks", "BadgeId", c => c.Int(nullable: false));
            CreateIndex("dbo.MissionTracks", "BadgeId");
            AddForeignKey("dbo.MissionTracks", "BadgeId", "dbo.Badges", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MissionTracks", "BadgeId", "dbo.Badges");
            DropIndex("dbo.MissionTracks", new[] { "BadgeId" });
            AlterColumn("dbo.MissionTracks", "BadgeId", c => c.Int());
            CreateIndex("dbo.MissionTracks", "BadgeId");
            AddForeignKey("dbo.MissionTracks", "BadgeId", "dbo.Badges", "Id");
        }
    }
}
