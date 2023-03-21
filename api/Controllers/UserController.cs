using api.Services;
using common.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly UserDb _context;

        // Use constructor dependency injection for the dependent services
        public UserController(UserDb context, 
            IEmailVerificationService emailVerificationService,
            IPasswordHasher passwordHasher)
        {
            _context = context;
            _emailVerificationService = emailVerificationService;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> Post(User user)        
        {            
            if(!_emailVerificationService.IsValidEmail(user.Email))
            {
                return BadRequest($"{user.Email} format incorrect");
            }

            // Check if the email address already exists in the database
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if(existingUser != null)
            {
                // Email already exists therefore return error message
                return BadRequest($"User already exists {user.Email}");
            }

            // Default starting balance of 100
            user.Balance = 100;

            // Hash the password to prevent it being stored in plain text
            user.HashedPassword = _passwordHasher.HashPassword(user.HashedPassword);

            // Add the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();                                            

            return new OkResult();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(string email, string password)
        {
            // Try and get the user by the supplied email address
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.HashedPassword == password);

            if(existingUser == null)
            {
                // User not found. Don't return the reason so hackers can't fish for valid email addresses
                return BadRequest($"Failed to login");
            }

            // Verifiy the supplied password
            if(!_passwordHasher.VerifyPassword(password, existingUser.HashedPassword))
            {
                // Password doesn't match the stored password                
                return BadRequest($"Failed to login");
            }

            return new OkObjectResult(existingUser);
        }
    }
}