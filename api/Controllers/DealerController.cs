using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DealerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly GameDb _GameContext;
        private readonly UserDb _UserContext;
        private readonly IGameEngine _gameEngine;

        public DealerController(GameDb gameContext, UserDb userContext, IGameEngine gameEngine, ILogger<PlayerController> logger)
        {
            _GameContext = gameContext;
            _UserContext = userContext;
            _gameEngine = gameEngine;
            _logger = logger;
        }

        [HttpPost("Play")]
        public async Task<ActionResult> Play(int gameId)
        {
            var gameEngine = new GameEngine();

            // Load all the related data in the query results we need for the bot to be able to play the hand
            var game = _GameContext.Games
                .Include(b => b.Bots).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Include(b => b.Bots).ThenInclude(b => b.Hand2).ThenInclude(x => x.Cards)
                .Include(d => d.Deck).ThenInclude(c => c.Cards)
                .Include(d => d.Dealer).ThenInclude(b => b.Hand).ThenInclude(x => x.Cards)
                .Include(p => p.Player).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Single(x => x.GameId == gameId);

            // Check the game was found
            if (game == null)
            {
                return BadRequest("No game found");
            }

            // Play the dealer hand
            _gameEngine.DealerPlay(game);

            // Find the player record from the user table
            var existingUser = await _UserContext.Users.FirstOrDefaultAsync(u => u.UserId == game.Player.UserId);

            // Check we found the player
            if (existingUser == null)
            {
                return BadRequest($"Unable to find user");
            }

            // Update the user and players balance
            existingUser.Balance += _gameEngine.CalculateWinnings(game.Player);
            game.Player.Balance = existingUser.Balance;

            // Save the user and game state
            await _UserContext.SaveChangesAsync();
            await _GameContext.SaveChangesAsync();

            return new OkObjectResult(game);
        }
    }
}