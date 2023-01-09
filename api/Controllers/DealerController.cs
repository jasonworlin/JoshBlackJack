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

        private readonly GameDb _context;
        public DealerController(GameDb context, ILogger<PlayerController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        [HttpPost("Play")]
        public async Task<ActionResult> Play(int gameId)
        {
            System.Console.WriteLine($"********************************************DEALER playing on game {gameId}");
            var gameEngine = new GameEngine();

            var game = _context.Games
                .Include(b => b.Bots).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Include(b => b.Bots).ThenInclude(b => b.Hand2).ThenInclude(x => x.Cards)
                .Include(d => d.Deck).ThenInclude(c => c.Cards)
                .Include(d => d.Dealer).ThenInclude(b => b.Hand).ThenInclude(x => x.Cards)
                .Include(p => p.Player).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Single(x => x.GameId == gameId);

            System.Console.WriteLine($"DEALER - GOT THE GAME");

            GameEngine.DealerPlay(game);

            System.Console.WriteLine($"Bots Won {game.Bots.Any(x => x.HasWon)}");

            await _context.SaveChangesAsync();

            return new OkObjectResult(game);
        }                
    }
}