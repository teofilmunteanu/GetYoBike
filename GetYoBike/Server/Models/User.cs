namespace GetYoBike.Server.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }

        //pt lucrurile private se fol camel case (modul in care scrii, formatul de scris)
        //ex: private string firstName (doar litera din mijloc e mare 

        public List<Rent> Rents { get; } = new();
    }
}
