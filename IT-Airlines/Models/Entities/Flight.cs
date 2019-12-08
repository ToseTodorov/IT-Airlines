using System;
using System.ComponentModel.DataAnnotations;

namespace IT_Airlines.Models.Entities
{
    public class Flight
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name ="Origin")]
        public virtual Airport AirportFrom { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public virtual Airport AirportTo { get; set; }

        [Required]
        public virtual Airplane Airplane { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Departure { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Landing { get; set; }

        [Required]
        [Display(Name ="Number of seats")]
        public int NumOfSeats { get; set; }

        [Required]
        [Display(Name = "Number of free seats")]
        public int NumOfFreeSeats { get; set; }

        [Required]
        public float BasePrice { get; set; }

        public override string ToString()
        {
            return string.Format("{1}-{2} - {3}$ - {4}", Id, AirportFrom.City, AirportTo.City, BasePrice, Departure);
        }
    }
}