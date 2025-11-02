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
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                Password = request.Password,
                Name = request.Name,
                Age = request.Age ?? 0,
                Bio = request.Bio,
                Location = request.Location,
                Zipcode = request.Zipcode,
                Gender = request.Gender,
                InterestedIn = request.InterestedIn
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
            var user = _users.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(user);
        }

        [HttpPost("{id}/photos")]
        public IActionResult AddPhoto(string id, [FromBody] string photoUrl)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            user.Photos.Add(photoUrl);
            return Ok();
        }
    }
}