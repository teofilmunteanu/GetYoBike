using GetYoBike.Server.Data;
using GetYoBike.Server.Entities;
using GetYoBike.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace GetYoBike.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikesController : ControllerBase
    {
        private readonly DataContext _context;

        public BikesController(DataContext context)
        {
            _context = context;
        }

        private Bike ModelToEntity(BikeModel bikeModel)
        {
            return new Bike()
            {
                Id = bikeModel.Id,
                TypeId = bikeModel.TypeId
            };
        }

        private BikeModel EntityToModel(Bike bike)
        {
            return new BikeModel()
            {
                Id = bike.Id,
                TypeId = bike.TypeId
            };
        }

        // GET: api/Bikes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BikeModel>>> GetBikes()
        {
            if (_context.Bikes == null)
            {
                return NotFound();
            }

            List<Bike> bikes = await _context.Bikes.ToListAsync();

            return Ok(bikes.Select(EntityToModel).ToList());
        }

        // GET: api/Bikes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BikeModel>> GetBike(int id)
        {
            if (_context.Bikes == null)
            {
                return NotFound();
            }

            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null)
            {
                return NotFound();
            }

            return Ok(EntityToModel(bike));
        }

        // PUT: api/Bikes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBike(int id, BikeModel bikeModel)
        {
            Bike bike = ModelToEntity(bikeModel);

            if (id != bike.Id)
            {
                return BadRequest();
            }

            _context.Entry(bike).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikeExists(id))
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

        // POST: api/Bikes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BikeModel>> PostBike(BikeModel bikeModel)
        {
            Bike bike = ModelToEntity(bikeModel);

            if (_context.Bikes == null)
            {
                return Problem("Entity set 'DataContext.Bikes'  is null.");
            }
            _context.Bikes.Add(bike);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBike", new { id = bike.Id }, bike);
        }

        // DELETE: api/Bikes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBike(int id)
        {
            if (_context.Bikes == null)
            {
                return NotFound();
            }
            var bike = await _context.Bikes.FindAsync(id);
            if (bike == null)
            {
                return NotFound();
            }

            _context.Bikes.Remove(bike);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BikeExists(int id)
        {
            return (_context.Bikes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //get list of bikes that are not rented in specified interval
        // GET: api/Bikes/availableBikesInInterval/dateTime?=2011-08-12T20:17:46.384Z&duration=5
        [HttpGet("availableBikesInInterval")]
        public async Task<ActionResult<List<Bike>>> GetAvailableBikes([BindRequired] string startDateTime, [BindRequired] string endDateTime)
        {
            DateTime startDateFormatted;
            DateTime endDateFormatted;
            List<Rent> rentsInInterval = new List<Rent>();
            List<Bike> availableBikes = new List<Bike>();

            bool startDateParseSuccessfuly = DateTime.TryParseExact(startDateTime, "yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateFormatted);
            bool endDateParseSuccessfuly = DateTime.TryParseExact(endDateTime, "yyyy-MM-dd-HH-mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateFormatted);

            if (startDateParseSuccessfuly && endDateParseSuccessfuly)
            {
                List<int> unavailableBikesIds = new List<int>();

                List<Rent> rents = /*await */_context.Rents.Include(r => r.RentedBike).ToList/*Async*/();

                //List<Rent> activeRents = rents.Where(r => r.RentStartDate.AddHours(r.RentHoursDuration) > dateTimeFormatted &&
                //    r.RentStartDate < dateTimeFormatted.AddHours((double)duration)).ToList();
                //activeRents.ForEach(r => unavailableBikesIds.Add(r.RentedBike.Id));

                List<Rent> activeRents = rents.Where(
                    r => r.StartDate >= startDateFormatted && r.EndDate <= endDateFormatted
                ).ToList();
                activeRents.ForEach(r => unavailableBikesIds.Add(r.RentedBike.Id));

                //check if any of the active rents has a bike, and add that bike to the list if not       
                availableBikes = _context.Bikes.Where(b => !(unavailableBikesIds.Contains(b.Id))).ToList();
            }
            else
            {
                return BadRequest("Invalid rent dates");
            }

            if (availableBikes.IsNullOrEmpty())
            {
                return NotFound();
            }
            //Console.WriteLine($"Setialized to universal format{dateTime.ToString("yyyy-MM-dd'T'HH:mm:ssZ")}"); 
            return Ok(availableBikes);
        }

        [HttpPut("changeBikeType/{id}")]
        public async Task<IActionResult> ChangeBikeType(int id, int bikeTypeId)
        {
            _context.Bikes.Include(b => b.Type);
            Bike bike = await _context.Bikes.FindAsync(id);
            if (bike == null)
            {
                return NotFound();
            }

            bike.TypeId = bikeTypeId;

            await _context.SaveChangesAsync();
            return Ok("Changed Bike Type");
        }
    }
}
