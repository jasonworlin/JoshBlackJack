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

        private readonly GameDb _context;
        public UserController(GameDb context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet(Name = "GetUser")]
        public ActionResult<Player> Get(int id)
        {
            System.Console.WriteLine("Getting a user");

            return new Player{
                Email = "email", Name = "myname", Password = "mypassword"
            };
        }

        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> Post(User user)        
        {
            var player = new Player(user.Name, user.Email, user.Password);
            System.Console.WriteLine($"Just got the user {player.Email} through");

            var existingUser = await _context.Players.FirstOrDefaultAsync(u => u.Email == player.Email);

            if(existingUser != null)
            {
                return BadRequest($"User already exists {player.Email}");
            }
            System.Console.WriteLine($"Adding user {player.Email}");

            // Default starting balance of 100
            player.Balance = 100;

            // TODO: Hash the password here
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
                    
                

            /* TODO: Check the user doesnt exist
            *  Add the user to the user database
            */ 

            return new OkResult();
        }
    }
}