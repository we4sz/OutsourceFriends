namespace OutsourceFriends.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingRequests", "MinAmount", c => c.Int(nullable: false));
            AddColumn("dbo.BookingRequests", "MaxAmount", c => c.Int(nullable: false));
            DropColumn("dbo.BookingRequests", "Amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BookingRequests", "Amount", c => c.Int(nullable: false));
            DropColumn("dbo.BookingRequests", "MaxAmount");
            DropColumn("dbo.BookingRequests", "MinAmount");
        }
    }
}
