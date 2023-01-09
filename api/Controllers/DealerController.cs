using api.Services;
using common.Domain;
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
        public DealerController(GameDb gameContext, UserDb userContext, ILogger<PlayerController> logger)
        {
            _GameContext = gameContext;
            _UserContext = userContext;
            _logger = logger;
        }

        [HttpPost("Play")]
        public async Task<ActionResult> Play(int gameId)
        {
            System.Console.WriteLine($"********************************************DEALER playing on game {gameId}");
            var gameEngine = new GameEngine();

            var game = _GameContext.Games
                .Include(b => b.Bots).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Include(b => b.Bots).ThenInclude(b => b.Hand2).ThenInclude(x => x.Cards)
                .Include(d => d.Deck).ThenInclude(c => c.Cards)
                .Include(d => d.Dealer).ThenInclude(b => b.Hand).ThenInclude(x => x.Cards)
                .Include(p => p.Player).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Single(x => x.GameId == gameId);

            System.Console.WriteLine($"DEALER - GOT THE GAME");

            GameEngine.DealerPlay(game);

            // TODO: This should be in the game engine really but it doesn't currently have a reference to the UserContext. Could pass it in?
            //if (game.Player.HasWon)
            {
                //Not saving the new balance
                var existingUser = await _UserContext.Users.FirstOrDefaultAsync(u => u.UserId == game.Player.UserId);

                if (existingUser == null)
                {
                    return BadRequest($"Unable to find user");
                }

                var winnings = GameEngine.CalculateWinnings(game.Player);
                System.Console.WriteLine($"WINNINGS {winnings}");

                System.Console.WriteLine($"Starting balance {existingUser.Balance}");
                existingUser.Balance += GameEngine.CalculateWinnings(game.Player);
                game.Player.Balance = existingUser.Balance;
                System.Console.WriteLine($"Closing balance {existingUser.Balance}");

                await _UserContext.SaveChangesAsync();
            }

            System.Console.WriteLine($"Bots Won {game.Bots.Any(x => x.HasWon)}");

            await _GameContext.SaveChangesAsync();

            return new OkObjectResult(game);
        }
    }
}