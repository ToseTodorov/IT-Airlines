namespace IT_Airlines.DataContexts.AirlineMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFlight1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Flights", "Departure", c => c.DateTime(nullable: false));
            AddColumn("dbo.Flights", "Landing", c => c.DateTime(nullable: false));
            AddColumn("dbo.Flights", "NumOfSeats", c => c.Int(nullable: false));
            AddColumn("dbo.Flights", "NumOfFreeSeats", c => c.Int(nullable: false));
            DropColumn("dbo.Flights", "TakeOffTime");
            DropColumn("dbo.Flights", "LandingTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Flights", "LandingTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Flights", "TakeOffTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Flights", "NumOfFreeSeats");
            DropColumn("dbo.Flights", "NumOfSeats");
            DropColumn("dbo.Flights", "Landing");
            DropColumn("dbo.Flights", "Departure");
        }
    }
}
