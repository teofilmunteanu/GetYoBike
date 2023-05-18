using System.ComponentModel.DataAnnotations;

namespace GetYoBike.Server.Entities
{
    public class Bike
    {
        public int Id { get; set; }

        [Required]
        public int TypeId { get; set; }

        [Required]
        public BikeType Type { get; set; }
        //tipurile sunt trecute deja in baza de date, pt a se putea crea legatura prin FK

        public ICollection<Rent>? Rents { get; set; }
    }
}
