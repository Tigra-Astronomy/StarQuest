namespace MS.Gamification.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObservingSessionCancellation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QueuedWorkItems", "Message", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QueuedWorkItems", "Message");
        }
    }
}
