using IT_Airlines.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace IT_Airlines.DataContexts
{
    public class AirlineDbContext : DbContext
    {
        
        public DbSet<Airplane> Airplanes { get; set; } 
        public DbSet<Airport> Airports { get; set; } 
        public DbSet<Flight> Flights { get; set; } 
        public DbSet<Reservation> Reservations { get; set; } 
        public DbSet<Luggage> Luggages { get; set; } 
        //public DbSet<Passenger> Passengers { get; set; }  

        public AirlineDbContext()
            : base("DefaultConnection")
        {
        }

        public static AirlineDbContext Create()
        {
            return new AirlineDbContext();
        }
    }
}