using GetYoBike.Shared.Models;

namespace GetYoBike.Client.Services
{
    public class RentService
    {
        public RentModel CurrentRent { get; private set; }

        public void SetRent(RentModel rent)
        {
            CurrentRent = rent;
        }
    }
}
