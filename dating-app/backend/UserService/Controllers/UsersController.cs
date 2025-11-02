using Microsoft.AspNetCore.Mvc;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private static List<User> _users = new List<User>
        {
            new User { 
                Id = "1", 
                Email = "test@test.com", 
                Password = Environment.GetEnvironmentVariable("DEFAULT_PASSWORD") ?? string.Empty, 
                Name = "Harry", 
                Age = 25,
                Bio = "Love hiking and coffee",
                Location = "New York",
                Zipcode = "10001",
                Gender = "Male",
                InterestedIn = "Female"
            }
        };

        [HttpPost]
        public ActionResult<User> CreateUser(CreateUserRequest request)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
                return BadRequest("Valid email is required");
            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
                return BadRequest("Password must be at least 6 characters");
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Name is required");

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email.Trim(),
                Password = request.Password,
                Name = request.Name.Trim(),
                Age = request.Age ?? 0,
                Bio = request.Bio?.Trim() ?? string.Empty,
                Location = request.Location?.Trim() ?? string.Empty,
                Zipcode = request.Zipcode?.Trim() ?? string.Empty,
                Gender = request.Gender?.Trim() ?? string.Empty,
                InterestedIn = request.InterestedIn?.Trim() ?? string.Empty
            };

            _users.Add(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(string id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return _users;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email and password are required");

            var user = _users.FirstOrDefault(u => u.Email.Equals(request.Email.Trim(), StringComparison.OrdinalIgnoreCase) && u.Password == request.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(user);
        }

        [HttpPost("{id}/photos")]
        public IActionResult AddPhoto(string id, [FromBody] string photoUrl)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User ID is required");
            if (string.IsNullOrWhiteSpace(photoUrl))
                return BadRequest("Photo URL is required");

            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            user.Photos.Add(photoUrl);
            return Ok();
        }
    }
}