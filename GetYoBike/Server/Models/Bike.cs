namespace GetYoBike.Server.Models
{
    //public enum BikeType
    //{
    //    city,
    //    mountain
    //}

    public abstract class Bike
    {
        public int Id { get; set; }
        //public BikeType Type { get; set; } //unde fac separarea de instantiere a.i. sa respect O/C Principle????

        public abstract decimal GetPrice();

        public List<Rent> Rents { get; } = new();
    }
}
