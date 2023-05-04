using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GetYoBike.Server.Data;
using GetYoBike.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Build.Framework;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GetYoBike.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'DataContext.Users'  is null.");
          }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPut("changeEmail/{id}")]
        public async Task<IActionResult> ChangeEmail(int id, string email)
        {
            //User user = _context.Users.Where(u => u.Id == id).First();
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = email;
            await _context.SaveChangesAsync();
            

            return Ok(user);
        }

        //POST postez ceva nou
        //PUT schimb ceva existent
        [HttpPut("changeFirstName/{id}")]//astea sunt link-uri
        public async Task<IActionResult> ChangeFirstName(int id, string name)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = name;
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPut("changeLastName/{id}")]
        public async Task<IActionResult> ChangeLastName(int id, string name)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.LastName = name;
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        //imi da ceva, nu schimba o informatie
        [HttpGet("checkEmail")]
        //ceea ce este in ghilimele este path-ul pe care trebuie sa l urmez 
        private bool ValidaterEmail(string email)//o alta modalitate in care sa spun ca e valid/invalid mailul, adica sa scrie
        {
            if(email == null)
                return false;
            return email.Contains("@");//returneaza adresa care incepe cu "@"
        }

        [HttpGet("checkAge")]
        private bool ValidaterAge(int age)
        {
            if (age<=14||age>=70)
                return false;
            return true;
        }
    }
}
