namespace GetYoBike.Server.Models
{
    public enum Types
    {
        city,
        mountain
    }

    public class BikeType
    {
        public int id;
        public decimal Price;
        public Types Type;
        public List<Bike> Bikes;
    }
}
