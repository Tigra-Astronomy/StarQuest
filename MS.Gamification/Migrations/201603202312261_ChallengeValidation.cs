namespace MS.Gamification.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChallengeValidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Challenges", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Challenges", "Name", c => c.String());
        }
    }
}
