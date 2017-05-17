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
                        SendNotifications = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "ObservingSession_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "ObservingSession_Id");
            AddForeignKey("dbo.AspNetUsers", "ObservingSession_Id", "dbo.ObservingSessions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "ObservingSession_Id", "dbo.ObservingSessions");
            DropIndex("dbo.AspNetUsers", new[] { "ObservingSession_Id" });
            DropColumn("dbo.AspNetUsers", "ObservingSession_Id");
            DropTable("dbo.ObservingSessions");
        }
    }
}
