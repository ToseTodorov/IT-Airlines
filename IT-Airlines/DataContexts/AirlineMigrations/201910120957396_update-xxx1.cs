namespace IT_Airlines.DataContexts.AirlineMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatexxx1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reservations", "FirstFlight_Id", "dbo.Flights");
            DropForeignKey("dbo.Reservations", "FirstLuggage_Id", "dbo.Luggages");
            DropIndex("dbo.Reservations", new[] { "FirstFlight_Id" });
            DropIndex("dbo.Reservations", new[] { "FirstLuggage_Id" });
            AlterColumn("dbo.Reservations", "FirstFlight_Id", c => c.Int());
            AlterColumn("dbo.Reservations", "FirstLuggage_Id", c => c.Int());
            CreateIndex("dbo.Reservations", "FirstFlight_Id");
            CreateIndex("dbo.Reservations", "FirstLuggage_Id");
            AddForeignKey("dbo.Reservations", "FirstFlight_Id", "dbo.Flights", "Id");
            AddForeignKey("dbo.Reservations", "FirstLuggage_Id", "dbo.Luggages", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "FirstLuggage_Id", "dbo.Luggages");
            DropForeignKey("dbo.Reservations", "FirstFlight_Id", "dbo.Flights");
            DropIndex("dbo.Reservations", new[] { "FirstLuggage_Id" });
            DropIndex("dbo.Reservations", new[] { "FirstFlight_Id" });
            AlterColumn("dbo.Reservations", "FirstLuggage_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Reservations", "FirstFlight_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Reservations", "FirstLuggage_Id");
            CreateIndex("dbo.Reservations", "FirstFlight_Id");
            AddForeignKey("dbo.Reservations", "FirstLuggage_Id", "dbo.Luggages", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Reservations", "FirstFlight_Id", "dbo.Flights", "Id", cascadeDelete: true);
        }
    }
}
