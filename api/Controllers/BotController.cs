using System.Net;
using api.Services;
using common.Domain;
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

        [HttpPost(Name = "TakeGo")]
        public async Task<ActionResult> TakeGo(int gameId)
        {
            System.Console.WriteLine($"********************************************Bot taking go on game {gameId}");
            // TODO: Rename function to Play
            var gameEngine = new GameEngine()/*{Game = game}*/;

            var game = _context.Games
                .Include(b => b.Bots).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Include(b => b.Bots).ThenInclude(b => b.Hand2).ThenInclude(x => x.Cards)
                .Include(d => d.Deck).ThenInclude(c => c.Cards)
                .Include(d => d.Dealer).ThenInclude(b => b.Hand).ThenInclude(x => x.Cards)
                .Include(p => p.Player).ThenInclude(b => b.Hand1).ThenInclude(x => x.Cards)
                .Single(x => x.GameId == gameId);

            if(game == null)
            {
                System.Console.WriteLine("NO GAME");
                return BadRequest();
            }

            // Game engine to work out whos turn it is nxt somehow.
            GameEngine.BotTakeATurn(game);

            await _context.SaveChangesAsync();

            return new OkObjectResult(game);
        }
    }
}