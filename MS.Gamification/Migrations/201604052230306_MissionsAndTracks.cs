 namespace MS.Gamification.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MissionsAndTracks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Missions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Level = c.Int(nullable: false),
                        AwardTitle = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MissionTracks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Number = c.Int(nullable: false),
                        AwardTitle = c.String(nullable: false),
                        Mission_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Missions", t => t.Mission_Id)
                .Index(t => t.Mission_Id);
            
            AddColumn("dbo.Challenges", "MissionTrack_Id", c => c.Int());
            CreateIndex("dbo.Challenges", "MissionTrack_Id");
            AddForeignKey("dbo.Challenges", "MissionTrack_Id", "dbo.MissionTracks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MissionTracks", "Mission_Id", "dbo.Missions");
            DropForeignKey("dbo.Challenges", "MissionTrack_Id", "dbo.MissionTracks");
            DropIndex("dbo.MissionTracks", new[] { "Mission_Id" });
            DropIndex("dbo.Challenges", new[] { "MissionTrack_Id" });
            DropColumn("dbo.Challenges", "MissionTrack_Id");
            DropTable("dbo.MissionTracks");
            DropTable("dbo.Missions");
        }
    }
}
