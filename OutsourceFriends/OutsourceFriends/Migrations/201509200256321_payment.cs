namespace OutsourceFriends.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class payment : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BookingRequests", new[] { "Accepted" });
            DropPrimaryKey("dbo.BookingRequests");
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
                .PrimaryKey(t => new { t.GuideId, t.TravelerId, t.BookingId, t.Id })
                .ForeignKey("dbo.BookingRequests", t => new { t.GuideId, t.TravelerId, t.BookingId }, cascadeDelete: true)
                .ForeignKey("dbo.Guides", t => t.Guide_UserId)
                .ForeignKey("dbo.Travelers", t => t.Traveler_UserId)
                .Index(t => new { t.GuideId, t.TravelerId, t.BookingId })
                .Index(t => t.Date)
                .Index(t => t.Guide_UserId)
                .Index(t => t.Traveler_UserId);
            
            AddColumn("dbo.BookingRequests", "Id", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.BookingRequests", "AcceptedDate", c => c.DateTime());
            AddColumn("dbo.BookingRequests", "TransactionId", c => c.String(maxLength: 256));
            AddPrimaryKey("dbo.BookingRequests", new[] { "GuideId", "TravelerId", "Id" });
            CreateIndex("dbo.BookingRequests", "MinAmount");
            CreateIndex("dbo.BookingRequests", "MaxAmount");
            CreateIndex("dbo.BookingRequests", "AcceptedDate");
            CreateIndex("dbo.BookingRequests", "TransactionId");
            DropColumn("dbo.BookingRequests", "Date");
            DropColumn("dbo.BookingRequests", "Accepted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BookingRequests", "Accepted", c => c.Boolean());
            AddColumn("dbo.BookingRequests", "Date", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.BookingDates", "Traveler_UserId", "dbo.Travelers");
            DropForeignKey("dbo.BookingDates", "Guide_UserId", "dbo.Guides");
            DropForeignKey("dbo.BookingDates", new[] { "GuideId", "TravelerId", "BookingId" }, "dbo.BookingRequests");
            DropIndex("dbo.BookingRequests", new[] { "TransactionId" });
            DropIndex("dbo.BookingRequests", new[] { "AcceptedDate" });
            DropIndex("dbo.BookingRequests", new[] { "MaxAmount" });
            DropIndex("dbo.BookingRequests", new[] { "MinAmount" });
            DropIndex("dbo.BookingDates", new[] { "Traveler_UserId" });
            DropIndex("dbo.BookingDates", new[] { "Guide_UserId" });
            DropIndex("dbo.BookingDates", new[] { "Date" });
            DropIndex("dbo.BookingDates", new[] { "GuideId", "TravelerId", "BookingId" });
            DropPrimaryKey("dbo.BookingRequests");
            DropColumn("dbo.BookingRequests", "TransactionId");
            DropColumn("dbo.BookingRequests", "AcceptedDate");
            DropColumn("dbo.BookingRequests", "Id");
            DropTable("dbo.BookingDates");
            AddPrimaryKey("dbo.BookingRequests", new[] { "GuideId", "TravelerId", "Date" });
            CreateIndex("dbo.BookingRequests", "Accepted");
        }
    }
}
