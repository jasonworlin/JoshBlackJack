using api.Services;
using common.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;

        private readonly GameDb _context;
        public PlayerController(GameDb context, ILogger<PlayerController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        [HttpPost("Hit")]
        public async Task<ActionResult> Hit(int gameId)
        {
            System.Console.WriteLine($"********************************************Player taking go on game {gameId}");

            var gameEngine = new GameEngine();

            var game = _context.Games
                .Include(b => b.Bots).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Include(b => b.Bots).ThenInclude(b => b.Hand2).ThenInclude(x => x.Cards)
                .Include(d => d.Deck).ThenInclude(c => c.Cards)
                .Include(d => d.Dealer).ThenInclude(b => b.Hand).ThenInclude(x => x.Cards)
                .Include(p => p.Player).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Single(x => x.GameId == gameId);

            GameEngine.PlayerTakeAHit(game);

            await _context.SaveChangesAsync();

            return new OkObjectResult(game);
        }

        [HttpPost("Stick")]
        public async Task<ActionResult> Stick(int gameId)
        {
            System.Console.WriteLine($"********************************************Player STICKIN on game {gameId}");
            var gameEngine = new GameEngine();

            var game = _context.Games
                .Include(b => b.Bots).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Include(b => b.Bots).ThenInclude(b => b.Hand2).ThenInclude(x => x.Cards)
                .Include(d => d.Deck).ThenInclude(c => c.Cards)
                .Include(d => d.Dealer).ThenInclude(b => b.Hand).ThenInclude(x => x.Cards)
                .Include(p => p.Player).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Single(x => x.GameId == gameId);
            
            game.Player.Sticking();

            await _context.SaveChangesAsync();

            return new OkObjectResult(game);
        }        
    }
}