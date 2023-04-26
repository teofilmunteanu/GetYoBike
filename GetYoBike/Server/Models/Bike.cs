namespace GetYoBike.Server.Models
{
    public enum BikeType
    {
        city,
        mountain
    }

    public class MountainBike : Bike
    {
        public override decimal GetPrice()
        {
            return 10;
        }
    }

    public class CityBike : Bike
    {
        public override decimal GetPrice()
        {
            return 5;
        }
    }

    public abstract class Bike
    {
        public int Id { get; set; }
        public BikeType Type { get; set; } //unde fac separarea de instantiere a.i. sa respect O/C Principle????

        public abstract decimal GetPrice();
    }
}
