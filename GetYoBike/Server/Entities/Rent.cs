using Microsoft.EntityFrameworkCore;

namespace GetYoBike.Server.Entities
{
    [PrimaryKey(nameof(UserID), nameof(BikeID))]
    public class Rent
    {
        public int UserID { get; set; }
        public int BikeID { get; set; }
        public DateTime RentStartDate { get; set; }
        public int RentHoursDuration { get; set; }

        public string CardNr { get; set; }
        public string CardExpMonth { get; set; }
        public string CardExpYear { get; set; }
        public string CardCVC { get; set; }

        public string PublicId { get; set; }

        public Bike RentedBike { get; set; }
        public User RenterUser { get; set; }
    }
}
