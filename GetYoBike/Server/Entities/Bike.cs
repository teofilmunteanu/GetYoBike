namespace GetYoBike.Server.Entities
{
    public class Bike
    {
        public int Id { get; set; }
        public BikeType Type { get; set; }
        //tipurile sunt trecute deja in baza de date, pt a se putea crea legatura prin FK

        public List<Rent> Rents;
    }
}
