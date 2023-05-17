using GetYoBike.Server.Data;
using GetYoBike.Server.Entities;
using GetYoBike.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        private Rent ModelToEntity(RentModel bikeTypeModel)
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
                CardCVC = bikeTypeModel.CardCVC,
                CardExpMonth = bikeTypeModel.CardExpMonth,
                CardExpYear = bikeTypeModel.CardExpYear,
                CardHolderName = bikeTypeModel.CardHolderName,
                EditPIN = bikeTypeModel.EditPIN
                //RenterUser = renterUser,
                //RentedBike = rentedBike
            };
        }

        private RentModel EntityToModel(Rent rent)
        {
            return new RentModel()
            {
                Id = rent.Id,
                UserID = rent.RenterUserId,
                BikeID = rent.RentedBikeId,
                RentStartDate = rent.RentStartDate,
                RentHoursDuration = rent.RentHoursDuration,
                CardNr = rent.CardNr,
                CardCVC = rent.CardCVC,
                CardExpMonth = rent.CardExpMonth,
                CardExpYear = rent.CardExpYear,
                CardHolderName = rent.CardHolderName,
                EditPIN = rent.EditPIN
            };
        }

        // GET: api/Rents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentModel>>> GetRents()
        {
            if (_context.Rents == null)
            {
                return NotFound();
            }

            List<Rent> rents = await _context.Rents.ToListAsync();

            return Ok(rents.Select(EntityToModel).ToList());
        }

        // GET: api/Rents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RentModel>> GetRent(int id)
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

            return Ok(EntityToModel(rent));
        }

        // PUT: api/Rents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        //functiile put sunt cele care dau UPDATE la ceva din DB
        public async Task<IActionResult> PutRent(int id, RentModel rentModel)
        {
            Rent rent = ModelToEntity(rentModel);

            if (id != rent.Id)
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
        public async Task<ActionResult<RentModel>> PostRent(RentModel rentModel)
        {
            Rent rent = ModelToEntity(rentModel);

            if (_context.Rents == null)
            {
                return Problem("Entity set 'DataContext.Rents'  is null.");
            }
            _context.Rents.Add(rent);
            await _context.SaveChangesAsync();

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

        private void DiscountApplier(Rent rent)
        {
            if (rent.RentHoursDuration > 4 && !rent.IsDiscounted)
            {
                rent.Price = rent.Price * 0.85m;
                //aplic discount-ul de 15%, 100-15=85
                rent.IsDiscounted = true;
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

        private bool DateCheck(Rent rent)
        {
            DateTime currentDate = DateTime.Now;
            if (rent.RentStartDate < currentDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPut("ChangeDate{'id'}")]
        public async Task<IActionResult> ChangeDate(int id, Rent rent)
        {
            //modific data, nu userii, deci inlocuiesc cu rents si lucrez pe rents
            var ToUpdateRent = await _context.Rents.FindAsync(id);
            if (ToUpdateRent == null)
            {
                return NotFound();
            }
            if (!DateCheck(rent))
            {
                ToUpdateRent.RentStartDate = rent.RentStartDate;
                await _context.SaveChangesAsync();
                return Ok(ToUpdateRent);
            }
            return BadRequest("Invalid rent start date.");
        }

        private int generatePIN()//creez un pin random intre 6 cifre 
        {
            Random generator = new Random();
            int PIN;
            do
            {
                PIN = generator.Next(100000, 999999);
            } while (IsPINTaken(PIN));
            return PIN;
        }

        private bool IsPINTaken(int pin)//returneaza true daca PIN-ul e deja luat
        {
            return true;
        }

        [HttpPut("ChangeDuration/{id}")]
        public async Task<IActionResult> ChangeDuration(int id, [BindRequired] int duration)
        {
            Rent rent = await _context.Rents.FindAsync(id);
            if(rent == null) 
            {
                return NotFound();
            } 
            if (duration <= 0)
            {
                return BadRequest("Duration should be more than 0");
            }
            rent.RentHoursDuration=duration;
            return Ok("Duration updated successfully");
        }

        [HttpGet("GetPrice/{id}")]
        public async Task<IActionResult> CalculatePrice(int id)
        {
            Rent rent = await _context.Rents.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            rent.Price = rent.RentHoursDuration * rent.RentedBike.Type.Price;
            DiscountApplier(rent);
            return Ok("Duration updated successfully");
        }

    }
    

}
