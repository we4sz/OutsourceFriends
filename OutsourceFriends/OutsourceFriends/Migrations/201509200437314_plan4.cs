namespace OutsourceFriends.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class plan4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingDates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BookingId = c.Long(nullable: false),
                        Date = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookingRequests", t => t.BookingId, cascadeDelete: true)
                .Index(t => t.BookingId)
                .Index(t => t.Date);
            
            CreateTable(
                "dbo.BookingPlanItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BookingId = c.Long(nullable: false),
                        Duration = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookingRequests", t => t.BookingId, cascadeDelete: true)
                .Index(t => t.BookingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookingPlanItems", "BookingId", "dbo.BookingRequests");
            DropForeignKey("dbo.BookingDates", "BookingId", "dbo.BookingRequests");
            DropIndex("dbo.BookingPlanItems", new[] { "BookingId" });
            DropIndex("dbo.BookingDates", new[] { "Date" });
            DropIndex("dbo.BookingDates", new[] { "BookingId" });
            DropTable("dbo.BookingPlanItems");
            DropTable("dbo.BookingDates");
        }
    }
}
