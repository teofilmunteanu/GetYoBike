using GetYoBike.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace GetYoBike.Server.Entities
{
    public class BikeType
    {
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public TypesModel Type { get; set; }

        public ICollection<Bike>? Bikes { get; set; }
    }
}
