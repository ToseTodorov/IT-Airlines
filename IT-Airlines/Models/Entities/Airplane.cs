using System.ComponentModel.DataAnnotations;

namespace IT_Airlines.Models.Entities
{
    public class Airplane
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }
        
        [Required]
        public int NumOfSeats { get; set; }

        public override string ToString()
        {
            return Code + " - " + Id;
        }
    }
}