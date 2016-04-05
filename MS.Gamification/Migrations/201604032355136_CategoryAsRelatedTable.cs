namespace MS.Gamification.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryAsRelatedTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Challenges", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Challenges", "CategoryId");
            AddForeignKey("dbo.Challenges", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
            DropColumn("dbo.Challenges", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Challenges", "Category", c => c.String());
            DropForeignKey("dbo.Challenges", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Challenges", new[] { "CategoryId" });
            DropColumn("dbo.Challenges", "CategoryId");
            DropTable("dbo.Categories");
        }
    }
}
