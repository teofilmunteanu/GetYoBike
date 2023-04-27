namespace GetYoBike.Server.Models
{
    public class MountainBike : Bike
    {
        public override decimal GetPrice()
        {
            return 10;
        }
    }
}
