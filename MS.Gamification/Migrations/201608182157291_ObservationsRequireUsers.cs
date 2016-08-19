namespace MS.Gamification.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObservationsRequireUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Observations", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Observations", new[] { "UserId" });
            AlterColumn("dbo.Observations", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Observations", "UserId");
            AddForeignKey("dbo.Observations", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Observations", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Observations", new[] { "UserId" });
            AlterColumn("dbo.Observations", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Observations", "UserId");
            AddForeignKey("dbo.Observations", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
