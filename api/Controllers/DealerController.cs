using api.Services;
using common.Domain;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DealerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;

        //private readonly GameDb _context;
        public DealerController(/*UserDb context, */ILogger<PlayerController> logger)
        {
           /* _context = context;*/
            _logger = logger;
        }
        
        [HttpPost("Play")]
        public ActionResult Play(Game game)
        {
            var gameEngine = new GameEngine();

            GameEngine.DealerPlay(game);

            //System.Console.WriteLine($"Bots Won {game.Bots.Any(x => x.HasWon)}");

            return new OkObjectResult(game);
        }                
    }
}