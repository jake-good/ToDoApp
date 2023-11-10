using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IUserSerivce _userService;

        public UserController(TodoContext userContext, IUserSerivce userSerivce)
        {
            _context = userContext;
            _userService = userSerivce;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet]
        public async Task<ActionResult<User[]>> GetUsers()
        {
            var users = await _context.Users.Include(u => u.TodoItems).ToArrayAsync();
            return users;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserRegistrationModel model)
        {
            var user = new User { Username = model.Username, HashedPassword = _userService.HashPassword(model.Password), Email = "fake@mail.com" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserRegistrationModel model)
        {
            if (_userService.IsUserValid(model.Username, model.Password))
            {
                var token = _userService.GenerateJwtToken(model.Username);
                var account = await _context.Users.FirstOrDefaultAsync(user => user.Username == model.Username);
                return Ok(new { Token = token, account?.UserId });
            }
            return Unauthorized("Invalid credentials");
        }

        [HttpDelete]
        public async Task<ActionResult<User>> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }
    }
}