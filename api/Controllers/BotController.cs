using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> _logger;

        private readonly GameDb _context;
        public BotController(GameDb context, ILogger<BotController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("Play")]
        public async Task<ActionResult> Play(int gameId)
        {
            var gameEngine = new GameEngine();

            // Load all the related data in the query results we need for the bot to be able to play the hand
            var game = _context.Games
                .Include(b => b.Bots).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Include(b => b.Bots).ThenInclude(b => b.Hand2).ThenInclude(x => x.Cards)
                .Include(d => d.Deck).ThenInclude(c => c.Cards)
                .Include(d => d.Dealer).ThenInclude(b => b.Hand).ThenInclude(x => x.Cards)
                .Include(p => p.Player).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Single(x => x.GameId == gameId);

            if (game == null)
            {
                return BadRequest("No game found");
            }

            GameEngine.BotPlayHand(game);

            await _context.SaveChangesAsync();

            return new OkObjectResult(game);
        }
    }
}