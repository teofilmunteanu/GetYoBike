using GetYoBike.Server.Data;
using GetYoBike.Server.Entities;
using GetYoBike.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

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
                RentStartDate = bikeTypeModel.RentStartDate,
                RentHoursDuration = bikeTypeModel.RentHoursDuration,
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

            if (rent.RentHoursDuration > 48)
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

        [HttpPut("changeDate{'id'}")]
        public async Task<IActionResult> ChangeDate(int id, Rent rent)
        {
            //modific data, nu userii, deci inlocuiesc cu rents si lucrez pe rents
            var ToUpdateRent = await _context.Rents.FindAsync(id);
            if (ToUpdateRent == null)
            {
                return NotFound();
            }
            if (!rent.DateCheck())
            {

                ToUpdateRent.RentStartDate = rent.RentStartDate;
                await _context.SaveChangesAsync();
                return Ok(ToUpdateRent);
            }
            return BadRequest("Invalid rent start date.");
        }

        [HttpPut("changeDuration/{id}")]
        public async Task<IActionResult> ChangeDuration(int id, [BindRequired] int duration)
        {
            Rent rent = await _context.Rents.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            if (duration <= 0)
            {
                return BadRequest("Duration should be more than 0");
            }
            rent.RentHoursDuration = duration;
            return Ok("Duration updated successfully");
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
            rent.Price = rent.RentHoursDuration * rent1.RentedBike.Type.Price;
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
