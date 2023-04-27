using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GetYoBike.Server.Data;
using GetYoBike.Server.Models;

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

        // GET: api/Rents/5/3
        [HttpGet("{userId}/{bikeId}")]
        public async Task<ActionResult<Rent>> GetRent(int userId, int bikeId)
        {
          if (_context.Rents == null)
          {
              return NotFound();
          }

          //might not work
            var rent = await _context.Rents.FindAsync(userId, bikeId);

            if (rent == null)
            {
                return NotFound();
            }

            return rent;
        }

        // PUT: api/Rents/5/3
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userId}/{bikeId}")]
        public async Task<IActionResult> PutRent(int userId, int bikeId, Rent rent)
        {
            if (userId != rent.UserID || bikeId != rent.BikeID)
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
        public async Task<ActionResult<Rent>> PostRent(Rent rent)
        {
          if (_context.Rents == null)
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

            return CreatedAtAction("GetRent", new { userId = rent.UserID, bikeId = rent.BikeID }, rent);
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

        //maybe change, how to map DateTime??
        // GET: api/Rents/5/3
        //[HttpGet]
        //public async Task<ActionResult<Rent>> GetAvailableRents(DateTime dateTime)
        //{
            //get list of bikes that are not rented in specified interval
            //return NotFound();
        //}
    }
}
