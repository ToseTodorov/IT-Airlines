﻿using System;
using System.ComponentModel.DataAnnotations;
using IT_Airlines.Models.Enums;

namespace IT_Airlines.Models.Entities
{
    public class Passenger
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Display (Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Required]
        public  Gender Gender { get; set; }

        [Display(Name = "Birthday")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Passport Code")]
        [Required]
        public string PassportCode { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName + " - " + PassportCode;
        }

    }
}