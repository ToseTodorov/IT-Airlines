namespace IT_Airlines.DataContexts.AirlineMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedReservations1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "UserAccount", c => c.Int(nullable: false));
            DropColumn("dbo.Reservations", "AccountId");
            DropColumn("dbo.Reservations", "FirstSeatRow");
            DropColumn("dbo.Reservations", "FirstSeatColumn");
            DropColumn("dbo.Reservations", "SecondSeatRow");
            DropColumn("dbo.Reservations", "SecondColumn");
            DropColumn("dbo.Passengers", "Country");
            DropColumn("dbo.Passengers", "Email");
            DropColumn("dbo.Passengers", "PhoneNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Passengers", "PhoneNumber", c => c.String(nullable: false));
            AddColumn("dbo.Passengers", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Passengers", "Country", c => c.String(nullable: false));
            AddColumn("dbo.Reservations", "SecondColumn", c => c.Int());
            AddColumn("dbo.Reservations", "SecondSeatRow", c => c.Int());
            AddColumn("dbo.Reservations", "FirstSeatColumn", c => c.Int(nullable: false));
            AddColumn("dbo.Reservations", "FirstSeatRow", c => c.Int(nullable: false));
            AddColumn("dbo.Reservations", "AccountId", c => c.Int(nullable: false));
            DropColumn("dbo.Reservations", "UserAccount");
        }
    }
}
