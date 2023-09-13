namespace GetYoBike.Shared.Models
{
    public class RentModel
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int BikeID { get; set; }

        public decimal Price { get; set; }

        //eventual separi rent model pe start-date, start-time, end-date, end-time ca sa fie consistent
        //cu ce are frontend-ul (dar ar tb dupa convertit intre ele si la trecerea ModelToEntity si invers)
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string CardNr { get; set; }
        public string CardCVC { get; set; }
        public string CardExpMonth { get; set; }
        public string CardExpYear { get; set; }
        public string CardHolderName { get; set; }

        public string EditPIN { get; set; }
    }
}
