using System.Transactions;
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
            Console.WriteLine($"\u001b[31mNew Game!\u001b[0m");
            // TESTING ONLY - take the user id from the NewGame passed in
            newGame.UserId = 1;
            System.Console.WriteLine($"Just got new game for user {newGame.UserId} through placing bet {newGame.BetPlaced}");

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

            System.Console.WriteLine($"API - Player Balance {game.Player.Balance}");

            //using (var scope = new TransactionScope())
            {
                _GameContext.Games.Add(game);
                await _GameContext.SaveChangesAsync();
                
                await _UserContext.SaveChangesAsync();

                //scope.Complete();
            }

            System.Console.WriteLine($"New game Id {game.GameId}");

            return new OkObjectResult(game);
        }
    }
}