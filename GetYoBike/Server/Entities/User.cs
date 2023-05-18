using System.ComponentModel.DataAnnotations;

namespace GetYoBike.Server.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public int Age { get; set; }

        public ICollection<Rent>? Rents { get; set; }

        public bool ValidaterEmail()
        {
            if (Email == null)
                return false;
            return Email.Contains("@");
        }

        public bool ValidaterAge()
        {
            if (Age <= 14 || Age >= 70)
                return false;
            return true;
        }
    }
}
