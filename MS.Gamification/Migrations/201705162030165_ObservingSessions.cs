namespace MS.Gamification.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObservingSessions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ObservingSessions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Venue = c.String(nullable: false),
                        StartsAt = c.DateTime(nullable: false),
                        Description = c.String(),
                        ScheduleState = c.Int(nullable: false),
                        RemindOneWeekBefore = c.Boolean(nullable: false),
                        RemindOneDayBefore = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QueuedWorkItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProcessAfter = c.DateTime(nullable: false),
                        QueueName = c.String(nullable: false, maxLength: 8),
                        Disposition = c.Int(nullable: false),
                        ObservingSessionId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ObservingSessions", t => t.ObservingSessionId)
                .Index(t => t.ProcessAfter)
                .Index(t => t.ObservingSessionId);
            
            AddColumn("dbo.AspNetUsers", "ObservingSession_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "ObservingSession_Id");
            AddForeignKey("dbo.AspNetUsers", "ObservingSession_Id", "dbo.ObservingSessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QueuedWorkItems", "ObservingSessionId", "dbo.ObservingSessions");
            DropForeignKey("dbo.AspNetUsers", "ObservingSession_Id", "dbo.ObservingSessions");
            DropIndex("dbo.QueuedWorkItems", new[] { "ObservingSessionId" });
            DropIndex("dbo.QueuedWorkItems", new[] { "ProcessAfter" });
            DropIndex("dbo.AspNetUsers", new[] { "ObservingSession_Id" });
            DropColumn("dbo.AspNetUsers", "ObservingSession_Id");
            DropTable("dbo.QueuedWorkItems");
            DropTable("dbo.ObservingSessions");
        }
    }
}
