namespace OutsourceFriends.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class plan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingPlanItems",
                c => new
                    {
                        GuideId = c.String(nullable: false, maxLength: 128),
                        TravelerId = c.String(nullable: false, maxLength: 128),
                        BookingId = c.Long(nullable: false),
                        Id = c.Long(nullable: false, identity: true),
                        Duration = c.Int(nullable: false),
                        Cost = c.Int(nullable: false),
                        Title = c.String(),
                        Guide_UserId = c.String(maxLength: 128),
                        Traveler_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GuideId, t.TravelerId, t.BookingId, t.Id })
                .ForeignKey("dbo.BookingRequests", t => new { t.GuideId, t.TravelerId, t.BookingId }, cascadeDelete: true)
                .ForeignKey("dbo.Guides", t => t.Guide_UserId)
                .ForeignKey("dbo.Travelers", t => t.Traveler_UserId)
                .Index(t => new { t.GuideId, t.TravelerId, t.BookingId })
                .Index(t => t.Guide_UserId)
                .Index(t => t.Traveler_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookingPlanItems", "Traveler_UserId", "dbo.Travelers");
            DropForeignKey("dbo.BookingPlanItems", "Guide_UserId", "dbo.Guides");
            DropForeignKey("dbo.BookingPlanItems", new[] { "GuideId", "TravelerId", "BookingId" }, "dbo.BookingRequests");
            DropIndex("dbo.BookingPlanItems", new[] { "Traveler_UserId" });
            DropIndex("dbo.BookingPlanItems", new[] { "Guide_UserId" });
            DropIndex("dbo.BookingPlanItems", new[] { "GuideId", "TravelerId", "BookingId" });
            DropTable("dbo.BookingPlanItems");
        }
    }
}
