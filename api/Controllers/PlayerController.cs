using api.Services;
using common.Domain;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;

        //private readonly GameDb _context;
        public PlayerController(/*GameDb context,*/ ILogger<PlayerController> logger)
        {
            /*_context = context;*/
            _logger = logger;
        }
        
        [HttpPost("Hit")]
        public ActionResult Hit(Game game)
        {
            var gameEngine = new GameEngine();

            GameEngine.PlayerTakeAHit(game);

            return new OkObjectResult(game);
        }

        [HttpPost("Stick")]
        public ActionResult Stick(Game game)
        {
            var gameEngine = new GameEngine();
            
            game.Player.Sticking();

            return new OkObjectResult(game);
        }        
    }
}