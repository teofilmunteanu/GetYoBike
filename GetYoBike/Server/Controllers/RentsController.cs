using GetYoBike.Server.Data;
using GetYoBike.Server.Entities;
using GetYoBike.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace GetYoBike.Server.Controllers
{
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
            User? renterUser = _context.Users.Find(bikeTypeModel.UserID);
            Bike? rentedBike = _context.Bikes.Find(bikeTypeModel.BikeID);

            if (renterUser == null || rentedBike == null)
            {
                return null;
            }
            //eventually throw exception to be catched below(PUT/POST) and return bad request with exception error msg

            return new Rent()
            {
                UserID = bikeTypeModel.UserID,
                BikeID = bikeTypeModel.BikeID,
                RentStartDate = bikeTypeModel.RentStartDate,
                RentHoursDuration = bikeTypeModel.RentHoursDuration,
                CardNr = bikeTypeModel.CardNr,
                CardExpMonth = bikeTypeModel.CardExpMonth,
                CardExpYear = bikeTypeModel.CardExpYear,
                CardCVC = bikeTypeModel.CardCVC,
                PublicId = bikeTypeModel.PublicId,
                RenterUser = renterUser,
                RentedBike = rentedBike
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
        [HttpGet("{bikeId}/{userId}")]
        public async Task<ActionResult<Rent>> GetRent(int userId, int bikeId)
        {
            if (_context.Rents == null)
            {
                return NotFound();
            }
            var rent = await _context.Rents.FindAsync(userId, bikeId);

            if (rent == null)
            {
                return NotFound();
            }

            return rent;
        }

        // PUT: api/Rents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{bikeId}/{userId}")]
        //functiile put sunt cele care dau UPDATE la ceva din DB
        public async Task<IActionResult> PutRent(int userId, int bikeId, RentModel rentModel)
        {
            Rent? rent = ModelToEntity(rentModel);

            if (rent == null || userId != rent.UserID || bikeId != rent.BikeID)
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
                if (!RentExists(userId, bikeId))
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
                if (RentExists(rent.UserID, rent.BikeID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRent", new { id = rent.UserID }, rent);
        }

        // DELETE: api/Rents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRent(int userId, int bikeId)
        {
            if (_context.Rents == null)
            {
                return NotFound();
            }
            var rent = await _context.Rents.FindAsync(userId, bikeId);
            if (rent == null)
            {
                return NotFound();
            }

            _context.Rents.Remove(rent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RentExists(int userId, int bikeId)
        {
            return (_context.Rents?.Any(e => e.UserID == userId && e.BikeID == bikeId)).GetValueOrDefault();
        }

        // GET: api/Rents/date?=2011-08-12T20:17:46.384Z (or just date)
        [HttpGet("date")]
        public async Task<ActionResult<List<Rent>>> GetAvailableRents([BindRequired] string dateTimeStr, [BindRequired] decimal duration)
        {
            DateTime dateTime;
            //returns bool
            DateTime.TryParse(dateTimeStr, out dateTime);

            //Console.WriteLine($"Setialized to universal format{dateTime.ToString("yyyy-MM-dd'T'HH:mm:ssZ")}"); 
            return NotFound();
            //get list of bikes that are not rented in specified interval
            //return NotFound();
        }

        [HttpGet("checkDiscount/{id}")]
        public async Task<bool> DiscountValidater(int id) //await se foloseste inside a non-async method
        {
            var rent = await _context.Rents.FindAsync(id);
            if (rent == null)
                return false;
            return true;
        }

    }

}
