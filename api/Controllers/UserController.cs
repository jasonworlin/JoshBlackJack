using common.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly UserDb _context;
        public UserController(UserDb context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetUser")]
        public ActionResult<User> Get(int id)
        {
            return new User{
                Email = "email", Name = "myname", Password = "mypassword"
            };
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> Post(User user)        
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if(existingUser != null)
            {
                return BadRequest($"User already exists {user.Email}");
            }

            // Default starting balance of 100
            user.Balance = 100;

            // TODO: Hash the password here
            _context.Users.Add(user);
            await _context.SaveChangesAsync();                                            

            return new OkResult();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(string email, string password)
        {
            // TODO: Hash password and check for match
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if(existingUser == null)
            {
                return BadRequest($"Failed to login");
            }

            return new OkObjectResult(existingUser);
        }
    }
}