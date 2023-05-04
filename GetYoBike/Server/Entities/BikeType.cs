namespace GetYoBike.Server.Models
{
    public class BikeType
    {
        public BikeType() { }
        public BikeType(int id, decimal price, Types type)
        {
            Id = id;
            Price = price;
            Type = type;
        }

        public int Id { get; set; }
        public decimal Price { get; set; }
        public Types Type { get; set; }
        public List<Bike> Bikes { get; }
    }
}
