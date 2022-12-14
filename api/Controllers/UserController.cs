using System.Net;
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
            System.Console.WriteLine("Getting a user");

            return new User{
                Email = "email", Name = "myname", Password = "mypassword"
            };
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> Post(User user)        
        {
            //var player = new Player(user.Name, user.Email, user.Password);
            //System.Console.WriteLine($"Just got the user {player.Email} through");

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if(existingUser != null)
            {
                return BadRequest($"User already exists {user.Email}");
            }
            System.Console.WriteLine($"Adding user {user.Email}");

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
            System.Console.WriteLine($"API - Loggin in {email}, {password}");
            // TODO: Hash password and check for match
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if(existingUser == null)
            {
                System.Console.WriteLine("Failed to login");
                return BadRequest($"Failed to login");
            }

            System.Console.WriteLine("API - Logged in!");
            return new OkObjectResult(existingUser);
        }
    }
}