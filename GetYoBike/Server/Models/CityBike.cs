namespace GetYoBike.Server.Models
{
    public class CityBike : Bike
    {
        public override decimal GetPrice()
        {
            return 5;
        }
    }
}
