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
            return new Rent()
            {
                Id = bikeTypeModel.Id,
                RenterUserId = bikeTypeModel.UserID,
                RentedBikeId = bikeTypeModel.BikeID,
                Price = bikeTypeModel.Price,
                StartDate = bikeTypeModel.StartDate,
                EndDate = bikeTypeModel.EndDate,
                CardNr = bikeTypeModel.CardNr,
                CardCVC = bikeTypeModel.CardCVC,
                CardExpMonth = bikeTypeModel.CardExpMonth,
                CardExpYear = bikeTypeModel.CardExpYear,
                CardHolderName = bikeTypeModel.CardHolderName,
                EditPIN = bikeTypeModel.EditPIN
            };
        }

        private RentModel EntityToModel(Rent rent)
        {
            return new RentModel()
            {
                Id = rent.Id,
                UserID = rent.RenterUserId,
                BikeID = rent.RentedBikeId,
                Price = rent.Price,
                StartDate = rent.StartDate,
                EndDate = rent.EndDate,
                CardNr = rent.CardNr,
                CardCVC = rent.CardCVC,
                CardExpMonth = rent.CardExpMonth,
                CardExpYear = rent.CardExpYear,
                CardHolderName = rent.CardHolderName,
                EditPIN = rent.EditPIN
            };
        }

        decimal GetDuration(Rent rent)
        {
            decimal DurationHours = (decimal)((rent.EndDate - rent.StartDate).TotalHours);
            return DurationHours;
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

        // GET: api/Rents/
        [HttpGet("getRentByDates")]
        public async Task<ActionResult<RentModel>> GetRent([BindRequired] int userId, [BindRequired] int bikeId, [BindRequired] string startDateTime, [BindRequired] string endDateTime)
        {
            if (_context.Rents == null)
            {
                return NotFound();
            }

            DateTime startDateFormatted;
            DateTime endDateFormatted;
            bool startDateParseSuccessfuly = DateTime.TryParseExact(startDateTime, "yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateFormatted);
            bool endDateParseSuccessfuly = DateTime.TryParseExact(endDateTime, "yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateFormatted);

            if (startDateParseSuccessfuly && endDateParseSuccessfuly)
            {
                //List<Rent> rentsWithBikes = await _context.Rents.Include(r => r.RentedBike).ToListAsync();
                Rent? rent = _context.Rents.Where(
                    r => r.RenterUserId == userId && r.RentedBikeId == bikeId && r.StartDate == startDateFormatted && r.EndDate == endDateFormatted
                ).FirstOrDefault();

                if (rent == null)
                {
                    return NotFound();
                }

                return Ok(EntityToModel(rent));
            }
            else
            {
                return BadRequest("Invalid rent dates");
            }
        }

        // PUT: api/Rents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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

                CalculatePrice(rent);
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

        // POST: api/Rents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RentModel>> PostRent(RentModel rentModel)
        {
            Rent rent = ModelToEntity(rentModel);

            if (_context.Rents == null)
            {
                return Problem("Entity set 'DataContext.Rents' is null.");
            }

            if (GetDuration(rent) > 48)
            {
                return BadRequest("Rent can't be longer than 48h");
            }

            if (!(rent.ValidateCardDate() && rent.ValidateCardholderName() &&
                rent.ValidateCardNumber() && rent.ValidateCVC()))
            {
                return BadRequest("Invalid card details! Payment refused.");
            }

            rent.EditPIN = generatePIN(rent.RenterUserId);

            _context.Rents.Add(rent);
            await _context.SaveChangesAsync();

            //after rent is loaded into context, price can be calculated based on RentedBike's type
            CalculatePrice(rent);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRent", new { id = rent.Id }, rent);
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

        [HttpGet("getRentsOfUser/{id}")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetRentsOfUser(int id)
        {
            User user = await _context.Users.Include(u => u.Rents).Where(u => u.Id == id).FirstAsync();

            if (user == null || user.Rents == null)
            {
                return NotFound();
            }

            return Ok((user.Rents).Select(EntityToModel).ToList());
        }


        private void CalculatePrice(Rent rent)
        {
            var rent1 = _context.Rents.Include(r => r.RentedBike).ThenInclude(b => b.Type).Where(r => r.Id == rent.Id).FirstOrDefault();
            rent.Price = GetDuration(rent) * rent1.RentedBike.Type.Price;
            rent.ApplyDiscount();
        }

        private string generatePIN(int id)//creez un pin random de 6 cifre pt user cu id
        {
            Random generator = new Random();
            int PIN;
            do
            {
                PIN = generator.Next(100000, 1000000);
            } while (IsPINTakenForUser(PIN.ToString(), id));

            return PIN.ToString();
        }

        private bool IsPINTakenForUser(string pin, int id)//returneaza true daca PIN-ul e deja luat
        {
            _context.Rents.Include(r => r.RenterUser);
            return _context.Rents.Where(r => r.RenterUser.Id == id).Any(r => r.EditPIN == pin);
        }
    }
}
