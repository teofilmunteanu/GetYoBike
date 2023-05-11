namespace GetYoBike.Server.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }

        //pt lucrurile private se fol camel case (modul in care scrii, formatul de scris)
        //ex: private string firstName (doar litera din mijloc e mare 

        public List<Rent> Rents { get; set; }
    }
}
