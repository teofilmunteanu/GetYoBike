using GetYoBike.Server.Data;
using GetYoBike.Server.Entities;
using GetYoBike.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        private User ModelToEntity(UserModel userModel)
        {
            return new User()
            {
                Id = userModel.Id,
                Email = userModel.Email,
                LastName = userModel.LastName,
                FirstName = userModel.FirstName,
                Age = userModel.Age
            };
        }

        private UserModel EntityToModel(User user)
        {
            return new UserModel()
            {
                Id = user.Id,
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Age = user.Age
            };
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            List<User> users = await _context.Users.ToListAsync();

            return Ok(users.Select(EntityToModel).ToList());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUser(int id)
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

            return Ok(EntityToModel(user));
        }

        [HttpGet("findUserByEmail/{email}")]
        public async Task<IActionResult> FindUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserModel userModel)
        {
            User user = ModelToEntity(userModel);

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
        public async Task<ActionResult<UserModel>> PostUser(UserModel userModel)
        {
            User user = ModelToEntity(userModel);

            if (_context.Users == null)
            {
                return Problem("Entity set 'DataContext.Users'  is null.");
            }

            if (!user.ValidaterEmail())
            {
                return BadRequest("Invalid email!");
            }

            if (_context.Users.Where(u => u.Email == userModel.Email).Any())
            {
                return Conflict("Email is already used!");
            }

            if (!user.ValidaterAge())
            {
                return BadRequest("User should be between 14 and 70 years old!");
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
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = email;
            await _context.SaveChangesAsync();


            return Ok(user);
        }

        [HttpPut("changeFirstName/{id}")]
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
    }
}
