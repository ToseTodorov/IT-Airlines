namespace IT_Airlines.DataContexts.AirlineMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedReservations3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "AccountEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reservations", "AccountEmail");
        }
    }
}
