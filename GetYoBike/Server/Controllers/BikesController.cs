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
    public class BikesController : ControllerBase
    {
        private readonly DataContext _context;

        public BikesController(DataContext context)
        {
            _context = context;
        }

        private Bike? ModelToEntity(BikeModel bikeModel)
        {
            BikeType? bikeType = _context.BikeTypes.Find(bikeModel.TypeId); //_context.BikeTypes.FirstOrDefault(b => b.Type == (Types)bikeModel.TypeId);
            if (bikeType == null)
            {
                return null;
            }
            //eventually throw exception to be catched below(PUT/POST) and return bad request with exception error msg

            return new Bike()
            {
                Id = bikeModel.Id,
                Type = bikeType
            };
        }

        // GET: api/Bikes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bike>>> GetBikes()
        {
            if (_context.Bikes == null)
            {
                return NotFound();
            }
            return await _context.Bikes.ToListAsync();
        }

        // GET: api/Bikes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bike>> GetBike(int id)
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

            return bike;
        }

        // PUT: api/Bikes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBike(int id, BikeModel bikeModel)
        {
            Bike? bike = ModelToEntity(bikeModel);

            if (bike == null || id != bike.Id)
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
        public async Task<ActionResult<Bike>> PostBike(BikeModel bikeModel)
        {
            Bike? bike = ModelToEntity(bikeModel);

            if (bike == null)
            {
                return BadRequest();
            }

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
        public async Task<ActionResult<List<Bike>>> GetAvailableBikes([BindRequired] string dateTime, [BindRequired] decimal duration)
        {
            DateTime dateTimeFormatted;
            List<Rent> rentsInInterval = new List<Rent>();
            List<Bike> availableBikes = new List<Bike>();

            if (DateTime.TryParse(dateTime, out dateTimeFormatted))
            {
                rentsInInterval = _context.Rents.Where(r => r.RentStartDate.AddHours(r.RentHoursDuration) > dateTimeFormatted && r.RentStartDate < dateTimeFormatted.AddHours((double)duration)).ToList();
                //verifica daca vreunul din renturi contine bike-ul acesta
                List<int> unavailableBikesIds = new List<int>();
                rentsInInterval.ForEach(r => unavailableBikesIds.Add(r.RentedBike.Id));

                availableBikes = _context.Bikes.Where(b => !(unavailableBikesIds.Contains(b.Id))).ToList();
            }
            else
            {
                throw new ArgumentException("Invalid date");
            }

            if (availableBikes == null)
            {
                return NotFound();
            }
            //Console.WriteLine($"Setialized to universal format{dateTime.ToString("yyyy-MM-dd'T'HH:mm:ssZ")}"); 
            return availableBikes;
        }
    }
}
