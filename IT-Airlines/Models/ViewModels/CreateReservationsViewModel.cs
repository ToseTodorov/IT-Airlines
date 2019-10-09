using IT_Airlines.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IT_Airlines.Models.ViewModels
{
    public class CreateReservationsViewModel
    {
        BeginReservationsViewModel PartialReservation { get; set; }

        Flight Flight { get; set; }
        Flight ReturnFlight { get; set; }
        List<Reservation> Reservations { get; set; }
    }
}