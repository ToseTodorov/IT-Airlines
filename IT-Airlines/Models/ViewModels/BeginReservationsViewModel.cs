using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IT_Airlines.Models.ViewModels
{
    public class BeginReservationsViewModel
    {
        [Required]
        [Display(Name ="Origin")]
        public int AirportFrom { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public int AirportTo { get; set; }

        [Required]
        [Display(Name = "Round Trip")]
        public bool RoundTrip { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Departure { get; set; }

        [DataType(DataType.Date)]
        public DateTime Return { get; set; }

        [Required]
        public int Passengers { get; set; }
    }
}