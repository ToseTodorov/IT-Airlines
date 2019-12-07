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

        public bool RoundTrip { get; set; }

        [Required]
        public int FirstLuggage { get; set; }

        public int? SecondLuggage { get; set; }

        [Required]
        public Passenger Passenger { get; set; }
    }
}