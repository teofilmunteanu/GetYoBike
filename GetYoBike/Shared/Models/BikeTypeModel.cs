namespace GetYoBike.Server.Models
{
    public enum Types
    {
        city,
        mountain
    }

    public class BikeTypeModel
    {
        public BikeTypeModel(int id, decimal price, Types type)
        {
            Id = id;
            Price = price;
            Type = type;
        }

        public int Id { get; set; }
        public decimal Price { get; set; }
        public Types Type { get; set; }
    }
}
