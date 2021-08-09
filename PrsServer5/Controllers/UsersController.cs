using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using PrsServer5.DTO;
using PrsServer5.Models;
using PrsServer5.ViewModel;

namespace PrsServer5.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context) {
            _context = context;
        }

        // GET: api/Users/login/{username}/{password}
        [HttpGet("{username}/{password}")]
        public async Task<ActionResult<User>> Login(string username, string password) {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Username.ToLower().Equals(username.ToLower())
                                        && u.Password.Equals(password));
            return (user == null)
                ? NotFound()
                : user;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserView>>> GetUsers() {
            return await _context.Users.Select(u => new UserView(u)).ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserView>> GetUser(int id) {
            var user = await _context.Users.FindAsync(id);

            if(user == null) {
                return NotFound();
            }

            return new UserView(user);
        }

        // POST: api/users/update/5
        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user) {
            return await PutUser(id, user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user) {
            if(id != user.Id) {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                if(!UserExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user) {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // REMOVE: api/Users/delete/5
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> RemoveUser(int id) {
            return await DeleteUser(id);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id) {
            var user = await _context.Users.FindAsync(id);
            if(user == null) {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id) {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}
