using api.Domain;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public ActionResult<Game> Get()
        {
            var g = new Game(2);
            g.NewGame();
            return Ok(new {jason = 1});
        }

        [HttpPost(Name = "PostWeatherForecast")]
        public void Post()
        {
            var g = new Game(2);
            g.NewGame();
        }
    }
}