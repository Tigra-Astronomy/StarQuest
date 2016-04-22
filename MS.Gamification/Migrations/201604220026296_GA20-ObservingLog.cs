namespace MS.Gamification.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GA20ObservingLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Observations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChallengeId = c.Int(nullable: false),
                        ObservationDateTimeUtc = c.DateTime(nullable: false),
                        Equipment = c.Int(nullable: false),
                        ObservingSite = c.String(),
                        Seeing = c.Int(nullable: false),
                        Transparency = c.Int(nullable: false),
                        Notes = c.String(),
                        ExpectedImage = c.String(),
                        SubmittedImage = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Challenges", t => t.ChallengeId, cascadeDelete: true)
                .Index(t => t.ChallengeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Observations", "ChallengeId", "dbo.Challenges");
            DropIndex("dbo.Observations", new[] { "ChallengeId" });
            DropTable("dbo.Observations");
        }
    }
}
