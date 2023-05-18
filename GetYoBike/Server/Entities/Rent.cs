using System.ComponentModel.DataAnnotations;

namespace GetYoBike.Server.Entities
{
    public class Rent
    {
        public int Id { get; set; }

        [Required]
        public int RenterUserId { get; set; }
        public User RenterUser { get; set; }

        [Required]
        public int RentedBikeId { get; set; }

        public Bike RentedBike { get; set; }

        public decimal Price { get; set; }
        public bool IsDiscounted { get; set; }

        [Required]
        public DateTime RentStartDate { get; set; }

        [Required]
        public int RentHoursDuration { get; set; }

        [Required]
        [MaxLength(16)]
        [MinLength(13)]
        public string CardNr { get; set; }

        [Required]
        [MaxLength(3)]
        [MinLength(4)]
        public string CardCVC { get; set; }

        [Required]
        public string CardExpMonth { get; set; }

        [Required]
        public string CardExpYear { get; set; }

        [Required]
        [MinLength(25)]
        public string CardHolderName { get; set; }

        public string EditPIN { get; set; }

        public void ApplyDiscount()
        {
            if (RentHoursDuration > 4 && !IsDiscounted)
            {
                Price = Price * 0.85m;
                //aplic discount-ul de 15%, 100-15=85
                IsDiscounted = true;
            }
        }

        public bool ValidateCardNumber()
        {
            //nr cardului e intre 13 si 16 cifre (1)
            if (CardNr.Length < 13 || CardNr.Length > 16)
            {
                return false;
            }
            // caracterul c(adica nr meu din card) este chiar o cifra, verific daca are caractere speciale in acele cifre
            foreach (char c in CardNr)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        public bool ValidateCardDate()
        {
            //fac parse la card date string si l transform intr-un obiect de tipul DateTime 
            DateTime expirationDate;
            //if (!DateTime.TryParseExact(CardExpMonth + "/" + CardExpYear, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out expirationDate))
            if (!DateTime.TryParse(CardExpYear + "-" + CardExpMonth, out expirationDate))
            {
                return false;
            }

            // aflu data curenta in care ne aflam
            DateTime currentDate = DateTime.Now;
            // compar datile
            if (expirationDate < currentDate)
            {
                return false;
            }
            return true;
        }

        public bool ValidateCardholderName()
        {
            //verific daca numele e empty sau nu
            if (string.IsNullOrWhiteSpace(CardHolderName))
            {
                return false;
            }
            // verific daca numele contine caractere speciale
            foreach (char c in CardHolderName)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-')
                {
                    return false;
                }
            }
            // !!maximul de caractere acceptate este 25!!
            if (CardHolderName.Length > 26)
            {
                return false;
            }
            return true;
        }

        public bool ValidateCVC()
        {
            //verific daca CVC-ul e null
            if (string.IsNullOrWhiteSpace(CardCVC))
            {
                return false;
            }

            // verific daca CVC-ul e facut doar din numere
            foreach (char c in CardCVC)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            //aparent, CVC-ul poate fi de lungime 3 sau 4
            if (CardCVC.Length < 3 || CardCVC.Length > 4)
            {
                return false;
            }
            return true;
        }

        public bool DateCheck()
        {
            DateTime currentDate = DateTime.Now;
            if (RentStartDate < currentDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
