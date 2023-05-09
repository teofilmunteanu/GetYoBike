namespace GetYoBike.Shared.Models
{
    public enum TypesModel
    {
        city,
        mountain
    }

    public class BikeTypeModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public TypesModel Type { get; set; }
    }
}
