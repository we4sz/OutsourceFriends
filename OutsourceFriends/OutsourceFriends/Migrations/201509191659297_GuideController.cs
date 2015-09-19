namespace OutsourceFriends.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GuideController : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GuideRatings",
                c => new
                    {
                        GuideId = c.String(nullable: false, maxLength: 128),
                        TravelerId = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Rating = c.Int(nullable: false),
                        Created = c.DateTime(),
                        LastEdited = c.DateTime(),
                        Guide_UserId = c.String(maxLength: 128),
                        Traveler_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GuideId, t.TravelerId })
                .ForeignKey("dbo.Guides", t => t.Guide_UserId)
                .ForeignKey("dbo.Travelers", t => t.Traveler_UserId)
                .Index(t => t.Guide_UserId)
                .Index(t => t.Traveler_UserId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        GuidId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 128),
                        Guid = c.Guid(nullable: false),
                        Guide_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GuidId, t.Name })
                .ForeignKey("dbo.Guides", t => t.Guide_UserId)
                .Index(t => t.Guide_UserId);
            
            AddColumn("dbo.Guides", "ShowInSearch", c => c.Boolean(nullable: false));
            AddColumn("dbo.Guides", "Title", c => c.String(maxLength: 256));
            AddColumn("dbo.Guides", "MinBudget", c => c.Int(nullable: false));
            CreateIndex("dbo.Guides", "ShowInSearch");
            CreateIndex("dbo.Guides", "MinBudget");
            DropColumn("dbo.Guides", "CurrentTitle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Guides", "CurrentTitle", c => c.String(maxLength: 256));
            DropForeignKey("dbo.GuideRatings", "Traveler_UserId", "dbo.Travelers");
            DropForeignKey("dbo.Tags", "Guide_UserId", "dbo.Guides");
            DropForeignKey("dbo.GuideRatings", "Guide_UserId", "dbo.Guides");
            DropIndex("dbo.Tags", new[] { "Guide_UserId" });
            DropIndex("dbo.Guides", new[] { "MinBudget" });
            DropIndex("dbo.Guides", new[] { "ShowInSearch" });
            DropIndex("dbo.GuideRatings", new[] { "Traveler_UserId" });
            DropIndex("dbo.GuideRatings", new[] { "Guide_UserId" });
            DropColumn("dbo.Guides", "MinBudget");
            DropColumn("dbo.Guides", "Title");
            DropColumn("dbo.Guides", "ShowInSearch");
            DropTable("dbo.Tags");
            DropTable("dbo.GuideRatings");
        }
    }
}
