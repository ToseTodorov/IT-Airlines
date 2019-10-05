using System.ComponentModel.DataAnnotations;

namespace IT_Airlines.Models.Entities
{
    public class Luggage
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int MaximumWeight { get; set; }

        [Required]
        public float Price { get; set; }

        public override string ToString()
        {
            return MaximumWeight + "kg - " + Price + "$";
        }

    }
}