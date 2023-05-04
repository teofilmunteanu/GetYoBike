namespace GetYoBike.Server.Models
{
    public class User
    {
        public User(int id, string email, string lastName, string firstName)
        {
            Id = id;
            Email = email;
            LastName = lastName;
            FirstName = firstName;
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }

        //pt lucrurile private se fol camel case (modul in care scrii, formatul de scris)
        //ex: private string firstName (doar litera din mijloc e mare 

        public List<Rent> Rents { get; } = new();
    }
}
