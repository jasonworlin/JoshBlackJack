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
        private readonly GameDb _GameContext;
        private readonly UserDb _UserContext;
        
        public GameController(GameDb gameContext, UserDb userContext, ILogger<GameController> logger)
        {
            _GameContext = gameContext;
            _UserContext = userContext;
            _logger = logger;
        }

        [HttpPost(Name = "NewGame")]
        public async Task<ActionResult> Post(NewGame newGame)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Green;
            // TODO TESTING ONLY - take the user id from the NewGame passed in
            newGame.UserId = 1;

            var existingUser = await _UserContext.Users.FirstOrDefaultAsync(u => u.UserId == newGame.UserId);

            if (existingUser == null)
            {
                return BadRequest($"Unable to find user");
            }

            var gameEngine = new GameEngine();
            var game = gameEngine.CreateGame(newGame);

            if (existingUser.Balance - newGame.BetPlaced < 0)
            {
                return BadRequest($"Insufficient funds");
            }

            existingUser.Balance -= newGame.BetPlaced;
            game.Player.Balance = existingUser.Balance;
            game.Player.BetPlaced = newGame.BetPlaced;

            _GameContext.Games.Add(game);
            await _GameContext.SaveChangesAsync();
            await _UserContext.SaveChangesAsync();

            return new OkObjectResult(game);
        }
    }
}