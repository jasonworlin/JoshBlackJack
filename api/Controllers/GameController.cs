using api.Services;
using common.Domain;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;

        private readonly GameDb _context;
        public GameController(GameDb context, ILogger<GameController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        [HttpPost(Name = "NewGame")]
        public async Task<ActionResult> Post(NewGame newGame)
        {
            newGame.UserId = 1; 
            System.Console.WriteLine($"Just got new game for user {newGame.UserId} through");

            var gameEngine = new GameEngine();
            var game = gameEngine.CreateGame(newGame);

            System.Console.WriteLine($"Game player hand {game.Player.Hand1 == null}");

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            System.Console.WriteLine($"New game Id {game.GameId}");

            return new OkObjectResult(game);
        }                
    }
}