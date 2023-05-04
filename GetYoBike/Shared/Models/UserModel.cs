namespace GetYoBike.Server.Models
{
    public class UserModel
    {
        public UserModel(int id, string email, string lastName, string firstName)
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
    }
}
