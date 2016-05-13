namespace MS.Gamification.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserToObservations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Observations", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Observations", "UserId");
            AddForeignKey("dbo.Observations", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Observations", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Observations", new[] { "UserId" });
            DropColumn("dbo.Observations", "UserId");
        }
    }
}
