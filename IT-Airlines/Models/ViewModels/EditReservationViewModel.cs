using IT_Airlines.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IT_Airlines.Models.ViewModels
{
    public class EditReservationViewModel
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Round Trip")]
        public bool RoundTrip { get; set; }

        [Required]
        [Display(Name = "First Luggage")]
        public int FirstLuggage { get; set; }

        [Display(Name = "Return Luggage")]
        public int? SecondLuggage { get; set; }

        [Required]
        public Passenger Passenger { get; set; }
    }
}