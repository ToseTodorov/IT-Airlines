using System.ComponentModel.DataAnnotations;

namespace IT_Airlines.Models.Entities
{
    public class Airport
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string City { get; set; }

        public override string ToString()
        {
            return Name + ", " + City;
        }
    }
}