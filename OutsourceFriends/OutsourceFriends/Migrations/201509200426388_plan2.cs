namespace OutsourceFriends.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class plan2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingPlanItems", "Amount", c => c.Int(nullable: false));
            DropColumn("dbo.BookingPlanItems", "Cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BookingPlanItems", "Cost", c => c.Int(nullable: false));
            DropColumn("dbo.BookingPlanItems", "Amount");
        }
    }
}
