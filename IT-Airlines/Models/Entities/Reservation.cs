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

        [Required]
        public bool RoundTrip { get; set; } // True ako e povraten bilet, false ako e vo eden pravec

        [Required]
        public virtual Flight FirstFlight { get; set; }

        public virtual Flight SecondFlight { get; set; }

        [Required]
        public virtual Luggage FirstLuggage { get; set; }

        public virtual Luggage SecondLuggage { get; set; }

    }
}