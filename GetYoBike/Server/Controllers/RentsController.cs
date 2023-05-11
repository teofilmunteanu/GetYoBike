using GetYoBike.Server.Data;
using GetYoBike.Server.Entities;
using GetYoBike.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GetYoBike.Server.Controllers
{

    //System.InvalidOperationException: No route matches the supplied values.
    [Route("api/[controller]")]
    [ApiController]
    public class RentsController : ControllerBase
    {
        private readonly DataContext _context;

        public RentsController(DataContext context)
        {
            _context = context;
        }

        private Rent? ModelToEntity(RentModel bikeTypeModel)
        {
            // Pun eu rentedUser si rentedBike sau se pune singure din DbContext?? NU TB SA SE PUNA IN DB
            //User? renterUser = _context.Users.Find(bikeTypeModel.UserID);
            //Bike? rentedBike = _context.Bikes.Find(bikeTypeModel.BikeID);

            //if (renterUser == null || rentedBike == null)
            //{
            //    return null;
            //}
            //eventually throw exception to be caught below(PUT/POST) and return bad request with exception error msg

            return new Rent()
            {
                Id = bikeTypeModel.Id,
                RenterUserId = bikeTypeModel.UserID,
                RentedBikeId = bikeTypeModel.BikeID,
                RentStartDate = bikeTypeModel.RentStartDate,
                RentHoursDuration = bikeTypeModel.RentHoursDuration,
                CardNr = bikeTypeModel.CardNr,
                CardExpMonth = bikeTypeModel.CardExpMonth,
                CardExpYear = bikeTypeModel.CardExpYear,
                CardCVC = bikeTypeModel.CardCVC,
                PublicId = bikeTypeModel.PublicId
                //RenterUser = renterUser,
                //RentedBike = rentedBike
            };
        }

        // GET: api/Rents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rent>>> GetRents()
        {
            if (_context.Rents == null)
            {
                return NotFound();
            }
            return await _context.Rents.ToListAsync();
        }

        // GET: api/Rents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rent>> GetRent(int id)
        {
            if (_context.Rents == null)
            {
                return NotFound();
            }
            var rent = await _context.Rents.FindAsync(id);

            if (rent == null)
            {
                return NotFound();
            }

            return rent;
        }

        // PUT: api/Rents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //functiile put sunt cele care dau UPDATE la ceva din DB
        public async Task<IActionResult> PutRent(int id, RentModel rentModel)
        {
            Rent? rent = ModelToEntity(rentModel);

            if (rent == null || id != rent.Id)
            {
                return BadRequest();
            }

            _context.Entry(rent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        //Error in API: No route matches the supplied values.

        // POST: api/Rents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Rent>> PostRent(RentModel rentModel)
        {
            Rent? rent = ModelToEntity(rentModel);

            if (rent == null)
            {
                return BadRequest();
            }

            if (_context.Rents == null)//mereu cand scrie "_context" inseamna ca se uita in baza de date dupa orice urmeaza dupa .
                                       //spre ex"_context.Rents" se uita in baza de date de la rents
            {
                return Problem("Entity set 'DataContext.Rents'  is null.");
            }
            _context.Rents.Add(rent);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RentExists(rent.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRent", new { id = rent.RenterUserId }, rent);
        }

        // DELETE: api/Rents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRent(int id)
        {
            if (_context.Rents == null)
            {
                return NotFound();
            }
            var rent = await _context.Rents.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }

            _context.Rents.Remove(rent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RentExists(int id)
        {
            return (_context.Rents?.Any(e => e.RenterUserId == id)).GetValueOrDefault();
        }

        private void DiscountApplier(Rent rent) //await se foloseste inside a non-async method
        {
            if (rent.RentHoursDuration > 4)
            { 
                //aplic discount
            }
        }

        private bool ValidateCardNumber(string cardNumber)
        {
            //nr cardului e intre 13 si 16 cifre (1)
            if (cardNumber.Length < 13 || cardNumber.Length > 16)
            {
                return false;
            }
            // caracterul c(adica nr meu din card) este chiar o cifra, verific daca are caractere speciale in acele cifre
            foreach (char c in cardNumber)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        private bool ValidateCardDate(string cardDate)
        {
            //fac parse la card date string si l transform intr-un obiect de tipul DateTime 
            DateTime expirationDate;
            if (!DateTime.TryParseExact(cardDate, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out expirationDate))
                return false;
            // aflu data curenta in care ne aflam
            DateTime currentDate = DateTime.Now;
            // compar datile
            if (expirationDate < currentDate)
            {
                return false;
            }
            return true;
        }

       private bool ValidateCardholderName(string cardName)
        {
            //verific daca numele e empty sau nu
            if (string.IsNullOrWhiteSpace(cardName))
            {
                return false;
            }
            // verific daca numele contine caractere speciale
            foreach (char c in cardName)
            {
                if (!char.IsLetter(c) && c != ' ' && c != '-')
                {
                    return false;
                }
            }
            // !!maximul de caractere acceptate este 25!!
            if (cardName.Length > 26)
            {
                return false;
            }
            return true;
        }

        private bool ValidateCVC(string CVC)
        {
            //verific daca CVC-ul e null
            if (string.IsNullOrWhiteSpace(CVC))
            {
                return false;
            }

            // verific daca CVC-ul e facut doar din numere
            foreach (char c in CVC)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            //aparent, CVC-ul poate fi de lungime 3 sau 4
            if (CVC.Length < 3 || CVC.Length > 4)
            {
                return false;
            }
            return true;
        }

    }

}
