using Microsoft.EntityFrameworkCore;

namespace GetYoBike.Server.Entities
{
    [PrimaryKey(nameof(RenterUserId), nameof(RentedBikeId))]
    public class Rent
    {
        public int RenterUserId { get; set; }
        public User RenterUser { get; set; }

        public int RentedBikeId { get; set; }
        public Bike RentedBike { get; set; }


        public DateTime RentStartDate { get; set; }
        public int RentHoursDuration { get; set; }

        public string CardNr { get; set; }
        public string CardExpMonth { get; set; }
        public string CardExpYear { get; set; }
        public string CardCVC { get; set; }

        public string PublicId { get; set; }

    }
}
