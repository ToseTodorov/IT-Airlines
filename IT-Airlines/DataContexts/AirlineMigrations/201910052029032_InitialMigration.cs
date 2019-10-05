namespace IT_Airlines.DataContexts.AirlineMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Airplanes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        NumOfSeats = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Airports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        City = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Flights",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TakeOffTime = c.DateTime(nullable: false),
                        LandingTime = c.DateTime(nullable: false),
                        BasePrice = c.Single(nullable: false),
                        Airplane_Id = c.Int(nullable: false),
                        AirportFrom_Id = c.Int(nullable: false),
                        AirportTo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Airplanes", t => t.Airplane_Id, cascadeDelete: false)
                .ForeignKey("dbo.Airports", t => t.AirportFrom_Id, cascadeDelete: false)
                .ForeignKey("dbo.Airports", t => t.AirportTo_Id, cascadeDelete: false)
                .Index(t => t.Airplane_Id)
                .Index(t => t.AirportFrom_Id)
                .Index(t => t.AirportTo_Id);
            
            CreateTable(
                "dbo.Luggages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MaximumWeight = c.Int(nullable: false),
                        Price = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Int(nullable: false),
                        RoundTrip = c.Boolean(nullable: false),
                        FirstSeatRow = c.Int(nullable: false),
                        FirstSeatColumn = c.Int(nullable: false),
                        SecondSeatRow = c.Int(),
                        SecondColumn = c.Int(),
                        FirstFlight_Id = c.Int(nullable: false),
                        FirstLuggage_Id = c.Int(nullable: false),
                        Passenger_Id = c.Int(nullable: false),
                        SecondFlight_Id = c.Int(),
                        SecondLuggage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flights", t => t.FirstFlight_Id, cascadeDelete: false)
                .ForeignKey("dbo.Luggages", t => t.FirstLuggage_Id, cascadeDelete: false)
                .ForeignKey("dbo.Passengers", t => t.Passenger_Id, cascadeDelete: false)
                .ForeignKey("dbo.Flights", t => t.SecondFlight_Id)
                .ForeignKey("dbo.Luggages", t => t.SecondLuggage_Id)
                .Index(t => t.FirstFlight_Id)
                .Index(t => t.FirstLuggage_Id)
                .Index(t => t.Passenger_Id)
                .Index(t => t.SecondFlight_Id)
                .Index(t => t.SecondLuggage_Id);
            
            CreateTable(
                "dbo.Passengers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        Gender = c.Int(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        PassportCode = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "SecondLuggage_Id", "dbo.Luggages");
            DropForeignKey("dbo.Reservations", "SecondFlight_Id", "dbo.Flights");
            DropForeignKey("dbo.Reservations", "Passenger_Id", "dbo.Passengers");
            DropForeignKey("dbo.Reservations", "FirstLuggage_Id", "dbo.Luggages");
            DropForeignKey("dbo.Reservations", "FirstFlight_Id", "dbo.Flights");
            DropForeignKey("dbo.Flights", "AirportTo_Id", "dbo.Airports");
            DropForeignKey("dbo.Flights", "AirportFrom_Id", "dbo.Airports");
            DropForeignKey("dbo.Flights", "Airplane_Id", "dbo.Airplanes");
            DropIndex("dbo.Reservations", new[] { "SecondLuggage_Id" });
            DropIndex("dbo.Reservations", new[] { "SecondFlight_Id" });
            DropIndex("dbo.Reservations", new[] { "Passenger_Id" });
            DropIndex("dbo.Reservations", new[] { "FirstLuggage_Id" });
            DropIndex("dbo.Reservations", new[] { "FirstFlight_Id" });
            DropIndex("dbo.Flights", new[] { "AirportTo_Id" });
            DropIndex("dbo.Flights", new[] { "AirportFrom_Id" });
            DropIndex("dbo.Flights", new[] { "Airplane_Id" });
            DropTable("dbo.Passengers");
            DropTable("dbo.Reservations");
            DropTable("dbo.Luggages");
            DropTable("dbo.Flights");
            DropTable("dbo.Airports");
            DropTable("dbo.Airplanes");
        }
    }
}
