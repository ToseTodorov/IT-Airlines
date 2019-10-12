using IT_Airlines.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_Airlines.Models.ViewModels
{
    public class CreateReservationsViewModel
    {
 //       public BeginReservationsViewModel PartialModel { get; set; }
        public bool RoundTrip { get; set; }

 //       public int NumPassengers { get; set; }

        public int Flight { get; set; }
        public int ReturnFlight { get; set; }

        public List<int> Luggages { get; set; }
        public List<int> ReturnLuggages { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}