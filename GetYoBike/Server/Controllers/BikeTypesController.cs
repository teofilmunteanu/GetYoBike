﻿using GetYoBike.Server.Data;
using GetYoBike.Server.Entities;
using GetYoBike.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GetYoBike.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public BikeTypesController(DataContext context)
        {
            _context = context;
        }

        private BikeType ModelToEntity(BikeTypeModel bikeTypeModel)
        {
            return new BikeType()
            {
                Id = bikeTypeModel.Id,
                Price = bikeTypeModel.Price,
                Type = bikeTypeModel.Type
            };
        }

        private BikeTypeModel EntityToModel(BikeType bikeType)
        {
            return new BikeTypeModel()
            {
                Id = bikeType.Id,
                Price = bikeType.Price,
                Type = (TypesModel)bikeType.Type
            };
        }

        // GET: api/BikeTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BikeTypeModel>>> GetBikeTypes()
        {
            if (_context.BikeTypes == null)
            {
                return NotFound();
            }

            List<BikeType> bikeTypes = await _context.BikeTypes.ToListAsync();

            return Ok(bikeTypes.Select(EntityToModel).ToList());
        }

        // GET: api/BikeTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BikeTypeModel>> GetBikeType(int id)
        {
            if (_context.BikeTypes == null)
            {
                return NotFound();
            }

            var bikeType = await _context.BikeTypes.FindAsync(id);
            if (bikeType == null)
            {
                return NotFound();
            }

            return Ok(EntityToModel(bikeType));
        }

        [HttpGet("getByName/{typeName}")]
        public async Task<ActionResult<BikeTypeModel>> GetBikeTypeByName(string typeName)
        {
            bool bikeTypeParsed = Enum.TryParse(typeName, out TypesModel bikeType);

            if (_context.BikeTypes == null || !bikeTypeParsed)
            {
                return NotFound();
            }

            //var bikeType = await _context.BikeTypes.FindAsync(id);
            BikeType? bikeTypeEntity = await _context.BikeTypes.Where(b => b.Type == bikeType).FirstOrDefaultAsync();
            if (bikeTypeEntity == null)
            {
                return NotFound();
            }

            return Ok(EntityToModel(bikeTypeEntity));
        }

        // PUT: api/BikeTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBikeType(int id, BikeTypeModel bikeTypeModel)
        {
            BikeType bikeType = ModelToEntity(bikeTypeModel);

            if (id != bikeType.Id)
            {
                return BadRequest();
            }

            _context.Entry(bikeType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BikeTypeExists(id))
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

        // POST: api/BikeTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BikeTypeModel>> PostBikeType(BikeTypeModel bikeTypeModel)
        {
            BikeType bikeType = ModelToEntity(bikeTypeModel);

            if (_context.BikeTypes == null)
            {
                return Problem("Entity set 'DataContext.BikeTypes'  is null.");
            }
            _context.BikeTypes.Add(bikeType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBikeType", new { id = bikeType.Id }, bikeType);
        }

        // DELETE: api/BikeTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBikeType(int id)
        {
            if (_context.BikeTypes == null)
            {
                return NotFound();
            }
            var bikeType = await _context.BikeTypes.FindAsync(id);
            if (bikeType == null)
            {
                return NotFound();
            }

            _context.BikeTypes.Remove(bikeType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BikeTypeExists(int id)
        {
            return (_context.BikeTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
