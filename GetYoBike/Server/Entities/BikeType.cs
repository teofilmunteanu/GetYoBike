using System.ComponentModel.DataAnnotations;

namespace GetYoBike.Server.Entities
{
    public enum Types
    {
        city,
        mountain
    }

    public class BikeType
    {
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public Types Type { get; set; }

        public ICollection<Bike>? Bikes { get; set; }
    }
}
