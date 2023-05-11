using Microsoft.EntityFrameworkCore;

namespace GetYoBike.Server.Entities
{
    public class Rent
    {
        public int Id { get; set; }
        public int RenterUserId { get; set; }
        public User RenterUser { get; set; }

        public int RentedBikeId { get; set; }
        public Bike RentedBike { get; set; }

        public decimal Price { get; set; } 
        public bool IsDiscounted { get; set; }

        public DateTime RentStartDate { get; set; }
        public int RentHoursDuration { get; set; }

        public string CardNr { get; set; }
        public string CardExpMonth { get; set; }
        public string CardExpYear { get; set; }
        public string CardCVC { get; set; }

        public string PublicId { get; set; }

    }
}
