namespace GetYoBike.Client.Services
{
    public class RentService
    {
        //public RentModel CurrentRent { get; private set; }
        public decimal Price { get; private set; } = 0;
        public decimal DurationHours { get; private set; } = 0;
        public static int MinRentDuration { get; } = 1;

        //public void SetRent(RentModel rent)
        //{
        //    CurrentRent = rent;
        //}

        public void SetDuration(DateTime startDateTime, DateTime endDateTime)
        {
            DurationHours = (decimal)((endDateTime - startDateTime).TotalHours);
        }

        public void CalculatePrice(decimal rentDuration, decimal pricePerH)
        {
            if (rentDuration >= 1)
            {
                Price = rentDuration * pricePerH;

                if (rentDuration > 4)
                {
                    Price *= 0.85m;
                }

                Price = Math.Round(Price, 2);
            }
        }
    }
}
