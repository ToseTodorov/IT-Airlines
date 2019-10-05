namespace IT_Airlines.DataContexts.AirlineMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IT_Airlines.DataContexts.AirlineDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataContexts\AirlineMigrations";
        }

        protected override void Seed(IT_Airlines.DataContexts.AirlineDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
