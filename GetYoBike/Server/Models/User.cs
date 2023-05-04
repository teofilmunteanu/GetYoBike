namespace GetYoBike.Server.Models
{
    public class User
    {
        private object value1;
        private object value2;
        private object value3;
        private object value4;

        public User(int id, string email, string lastName, string firstName)
        {
            this.Id = id;
            this.Email = email;
            this.LastName = lastName;
            this.FirstName = firstName;
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        //pt lucrurile private se fol camel case (modul in care scrii, formatul de scris)
        //ex: private string firstName (doar litera din mijloc e mare 

        public List<Rent> Rents { get; } = new();
    }
}
