﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IT_Airlines.Models.ViewModels
{
    public class FlightViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Origin")]
        public int AirportFrom { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public int AirportTo { get; set; }

        [Required]
        public int Airplane { get; set; }

        //[Required]
        //[DataType(DataType.DateTime)]
        public String Departure { get; set; }

        //[Required]
        //[DataType(DataType.DateTime)]
        public String Landing { get; set; }

        [Required]
        [Display(Name = "Price")]
        public float BasePrice { get; set; }
    }
}