namespace IT_Airlines.DataContexts.AirlineMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedReservations2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reservations", "UserAccount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reservations", "UserAccount", c => c.Int(nullable: false));
        }
    }
}
