using System.ComponentModel.DataAnnotations;
using IT_Airlines.Models.Enums;

namespace IT_Airlines.Models.Entities
{
    public class Reservation
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public int AccountId { get; set; }

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

        [Required]
        public int FirstSeatRow { get; set; }
        [Required]
        public int FirstSeatColumn { get; set; }

        public int? SecondSeatRow { get; set; }
        public int? SecondColumn { get; set; }


        /*
        public float CalculatePrice()
        {
            float finalPrice = SumPrice();

            return finalPrice;
        }

        protected float SumPrice()
        {
            float sum = FirstFlight.BasePrice + FirstLuggage.Price;

            if (FirstSeatType == SeatType.FirstClass)
                sum += 20f;
            else if (FirstSeatType == SeatType.BusinessClass)
                sum += 10f;

            if (RoundTrip)
            {
                sum += SecondFlight.BasePrice + SecondLuggage.Price;

                if (SecondSeatType == SeatType.FirstClass)
                    sum += 20f;
                else if (SecondSeatType == SeatType.BusinessClass)
                    sum += 10f;
            }
            return sum;
        }
        */
    }
}