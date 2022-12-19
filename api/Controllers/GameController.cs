using System.Net;
using api.Services;
using common.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;

        private readonly UserDb _context;
        public GameController(UserDb context, ILogger<GameController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /*[HttpGet(Name = "GetUser")]
        public ActionResult<User> Get(int id)
        {
            System.Console.WriteLine("Getting a user");

            return new User{
                Email = "email", Name = "myname", Password = "mypassword"
            };
        }*/

        [HttpPost(Name = "NewGame")]
        public ActionResult Post(NewGame newGame)
        {
            System.Console.WriteLine($"Just got new game for user {newGame.UserId} through");

            //return BadRequest($"User already exists {user.Email}");
            
            // TODO: Hash the password here
            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();

            var gameEngine = new GameEngine();
            var game = gameEngine.CreateGame(newGame.NumberOfBots);

            //var g = new Game();
            //var players = g.NewGame(game.NumberOfBots); 

            //System.Console.WriteLine($"Players created {players.Count()}");



            /* TODO: Check the user doesnt exist
            *  Add the user to the user database
            */

            return new OkObjectResult(game);
        }

        [HttpPut(Name = "TakeGo")]
        public ActionResult Put(Game game)
        {
            var gameEngine = new GameEngine()/*{Game = game}*/;

            // Game engine to work out whos turn it is nxt somehow.
            GameEngine.TakeATurn(game);

            return new OkObjectResult(game/*gameEngine.Game*/);
        }

        public class NewGame
        {
            public int UserId { get; set; }
            public int NumberOfBots { get; set; }
        }
    }
}