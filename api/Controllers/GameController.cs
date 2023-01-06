using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;

        /*private readonly UserDb _context;*/
        public GameController(/*UserDb context,*/ ILogger<GameController> logger)
        {
            /*_context = context;*/
            _logger = logger;
        }
        
        [HttpPost(Name = "NewGame")]
        public ActionResult Post(NewGame newGame)
        {
            System.Console.WriteLine($"Just got new game for user {newGame.UserId} through");

            var gameEngine = new GameEngine();
            var game = gameEngine.CreateGame(newGame.NumberOfBots);

            return new OkObjectResult(game);
        }

        
        public class NewGame
        {
            public int UserId { get; set; }
            public int NumberOfBots { get; set; }
        }
    }
}