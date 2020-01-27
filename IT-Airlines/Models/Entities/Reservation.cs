using System.ComponentModel.DataAnnotations;
using IT_Airlines.Models.Enums;

namespace IT_Airlines.Models.Entities
{
    public class Reservation
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string AccountEmail { get; set; }

        [Required]
        public Passenger Passenger { get; set; }

        //[Required]
        public bool RoundTrip { get; set; } // True ako e povraten bilet, false ako e vo eden pravec

        //[Required]
        [Display(Name = "First Flight")]
        public virtual Flight FirstFlight { get; set; }

        [Display(Name = "Return Flight")]
        public virtual Flight SecondFlight { get; set; }

        //[Required]
        [Display(Name = "First Luggage")]
        public virtual Luggage FirstLuggage { get; set; }

        [Display(Name = "Return Luggage")]
        public virtual Luggage SecondLuggage { get; set; }

    }
}