namespace GetYoBike.Server.Models
{
    //public enum BikeType
    //{
    //    city,
    //    mountain
    //}

    //public class MountainBike : Bike
    //{
    //    public decimal GetPrice()
    //    {
    //        return 10;
    //    }
    //}

    //public class CityBike : Bike
    //{
    //    public decimal GetPrice()
    //    {
    //        return 5;
    //    }
    //}

    public class Bike
    {
        public int Id { get; set; }
        //public BikeType Type { get; set; } //unde fac separarea de instantiere a.i. sa respect O/C Principle????

        //public decimal GetPrice();

        public List<Rent> Rents { get; } = new();
    }
}
