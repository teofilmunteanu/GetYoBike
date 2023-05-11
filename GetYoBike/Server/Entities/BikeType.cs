namespace GetYoBike.Server.Entities
{
    public enum Types
    {
        city,
        mountain
    }

    public class BikeType
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public Types Type { get; set; }
        public List<Bike> Bikes { get; set; }
    }
}
