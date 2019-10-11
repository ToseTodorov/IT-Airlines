using IT_Airlines.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_Airlines.Models.ViewModels
{
    public class CreateReservationsViewModel
    {
        public BeginReservationsViewModel PartialReservation { get; set; }

        public int Flight { get; set; }
        public int ReturnFlight { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}