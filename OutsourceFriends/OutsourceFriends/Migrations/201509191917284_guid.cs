namespace OutsourceFriends.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class guid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingRequests",
                c => new
                    {
                        GuideId = c.String(nullable: false, maxLength: 128),
                        TravelerId = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Int(nullable: false),
                        Accepted = c.Boolean(),
                        Created = c.DateTime(),
                        Guide_UserId = c.String(maxLength: 128),
                        Traveler_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.GuideId, t.TravelerId, t.Date })
                .ForeignKey("dbo.Guides", t => t.Guide_UserId)
                .ForeignKey("dbo.Travelers", t => t.Traveler_UserId)
                .Index(t => t.Accepted)
                .Index(t => t.Created)
                .Index(t => t.Guide_UserId)
                .Index(t => t.Traveler_UserId);
            
            CreateTable(
                "dbo.TravelerRatings",
                c => new
                    {
                        TravelerId = c.String(nullable: false, maxLength: 128),
                        GuideId = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                        Rating = c.Int(nullable: false),
                        Created = c.DateTime(),
                        LastEdited = c.DateTime(),
                        Guide_UserId = c.String(maxLength: 128),
                        Traveler_UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.TravelerId, t.GuideId })
                .ForeignKey("dbo.Guides", t => t.Guide_UserId)
                .ForeignKey("dbo.Travelers", t => t.Traveler_UserId)
                .Index(t => t.Guide_UserId)
                .Index(t => t.Traveler_UserId);
            
            DropColumn("dbo.Travelers", "CurrentTitle");

            Sql(@"CREATE SPATIAL INDEX SPINDEX_GUIDE
                ON Guides(Location);

                CREATE SPATIAL INDEX SPINDEX_TRAVELER
                ON Travelers(Location);");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Travelers", "CurrentTitle", c => c.String(maxLength: 256));
            DropForeignKey("dbo.TravelerRatings", "Traveler_UserId", "dbo.Travelers");
            DropForeignKey("dbo.TravelerRatings", "Guide_UserId", "dbo.Guides");
            DropForeignKey("dbo.BookingRequests", "Traveler_UserId", "dbo.Travelers");
            DropForeignKey("dbo.BookingRequests", "Guide_UserId", "dbo.Guides");
            DropIndex("dbo.TravelerRatings", new[] { "Traveler_UserId" });
            DropIndex("dbo.TravelerRatings", new[] { "Guide_UserId" });
            DropIndex("dbo.BookingRequests", new[] { "Traveler_UserId" });
            DropIndex("dbo.BookingRequests", new[] { "Guide_UserId" });
            DropIndex("dbo.BookingRequests", new[] { "Created" });
            DropIndex("dbo.BookingRequests", new[] { "Accepted" });
            DropTable("dbo.TravelerRatings");
            DropTable("dbo.BookingRequests");
        }
    }
}
