namespace OutsourceFriends.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class plan3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BookingPlanItems", new[] { "GuideId", "TravelerId", "BookingId" }, "dbo.BookingRequests");
            DropForeignKey("dbo.BookingPlanItems", "Guide_UserId", "dbo.Guides");
            DropForeignKey("dbo.BookingPlanItems", "Traveler_UserId", "dbo.Travelers");
            DropForeignKey("dbo.BookingDates", new[] { "GuideId", "TravelerId", "BookingId" }, "dbo.BookingRequests");
            DropForeignKey("dbo.BookingDates", "Guide_UserId", "dbo.Guides");
            DropForeignKey("dbo.BookingDates", "Traveler_UserId", "dbo.Travelers");
            DropIndex("dbo.BookingDates", new[] { "GuideId", "TravelerId", "BookingId" });
            DropIndex("dbo.BookingDates", new[] { "Date" });
            DropIndex("dbo.BookingDates", new[] { "Guide_UserId" });
            DropIndex("dbo.BookingDates", new[] { "Traveler_UserId" });
            DropIndex("dbo.BookingPlanItems", new[] { "GuideId", "TravelerId", "BookingId" });
            DropIndex("dbo.BookingPlanItems", new[] { "Guide_UserId" });
            DropIndex("dbo.BookingPlanItems", new[] { "Traveler_UserId" });
            DropPrimaryKey("dbo.BookingRequests");
            AlterColumn("dbo.BookingRequests", "GuideId", c => c.String(nullable: false));
            AlterColumn("dbo.BookingRequests", "TravelerId", c => c.String(nullable: false));
            AddPrimaryKey("dbo.BookingRequests", "Id");
            DropTable("dbo.BookingDates");
            DropTable("dbo.BookingPlanItems");
        }
        
        public override void Down()
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
                        Amount = c.Int(nullable: false),
                        Title = c.String(),
                        Guide_UserId = c.String(maxLength: 128),
                        Traveler_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GuideId, t.TravelerId, t.BookingId, t.Id });
            
            CreateTable(
                "dbo.BookingDates",
                c => new
                    {
                        GuideId = c.String(nullable: false, maxLength: 128),
                        TravelerId = c.String(nullable: false, maxLength: 128),
                        BookingId = c.Long(nullable: false),
                        Id = c.Long(nullable: false, identity: true),
                        Date = c.DateTime(),
                        Guide_UserId = c.String(maxLength: 128),
                        Traveler_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GuideId, t.TravelerId, t.BookingId, t.Id });
            
            DropPrimaryKey("dbo.BookingRequests");
            AlterColumn("dbo.BookingRequests", "TravelerId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.BookingRequests", "GuideId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.BookingRequests", new[] { "GuideId", "TravelerId", "Id" });
            CreateIndex("dbo.BookingPlanItems", "Traveler_UserId");
            CreateIndex("dbo.BookingPlanItems", "Guide_UserId");
            CreateIndex("dbo.BookingPlanItems", new[] { "GuideId", "TravelerId", "BookingId" });
            CreateIndex("dbo.BookingDates", "Traveler_UserId");
            CreateIndex("dbo.BookingDates", "Guide_UserId");
            CreateIndex("dbo.BookingDates", "Date");
            CreateIndex("dbo.BookingDates", new[] { "GuideId", "TravelerId", "BookingId" });
            AddForeignKey("dbo.BookingDates", "Traveler_UserId", "dbo.Travelers", "UserId");
            AddForeignKey("dbo.BookingDates", "Guide_UserId", "dbo.Guides", "UserId");
            AddForeignKey("dbo.BookingDates", new[] { "GuideId", "TravelerId", "BookingId" }, "dbo.BookingRequests", new[] { "GuideId", "TravelerId", "Id" }, cascadeDelete: true);
            AddForeignKey("dbo.BookingPlanItems", "Traveler_UserId", "dbo.Travelers", "UserId");
            AddForeignKey("dbo.BookingPlanItems", "Guide_UserId", "dbo.Guides", "UserId");
            AddForeignKey("dbo.BookingPlanItems", new[] { "GuideId", "TravelerId", "BookingId" }, "dbo.BookingRequests", new[] { "GuideId", "TravelerId", "Id" }, cascadeDelete: true);
        }
    }
}
