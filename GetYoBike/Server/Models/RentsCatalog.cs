namespace GetYoBike.Server.Models
{
    public class RentsCatalog
    {
        public string UserID { get; set; }
        public string BikeID { get; set; }
        public DateTime RentStartDate { get; set; }
        public int RentHoursDuration { get; set; }
        public string CardNr { get; set; }
        public string CardExpMonth { get; set; }
        public string CardExpYear { get; set; }
        public string CardCVC { get; set; }
    }
}
