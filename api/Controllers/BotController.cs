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

        /*private readonly UserDb _context;*/
        public BotController(/*UserDb context, */ILogger<BotController> logger)
        {
            /*_context = context;*/
            _logger = logger;
        }

        [HttpPost(Name = "TakeGo")]
        public ActionResult Post(Game game)
        {
            // TODO: Rename function to Play
            var gameEngine = new GameEngine()/*{Game = game}*/;

            // Game engine to work out whos turn it is nxt somehow.
            GameEngine.BotTakeATurn(game);

            return new OkObjectResult(game/*gameEngine.Game*/);
        }
    }
}