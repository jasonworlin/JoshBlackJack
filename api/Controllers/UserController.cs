using api.Domain;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetUser")]
        public ActionResult<User> Get(int id)
        {
            System.Console.WriteLine("Getting a user");

            return new User{
                Email = "email", Name = "myname", Password = "mypassword"
            };
        }

        [HttpPost(Name = "CreateUser")]
        public void Post(User user)
        {
            System.Console.WriteLine($"Just got the user {user.Email} through");
        }
    }
}