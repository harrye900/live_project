using Microsoft.AspNetCore.Mvc;
using MatchService.Models;

namespace MatchService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private static List<Match> _matches = new List<Match>();
        private static List<UserProfile> _users = new List<UserProfile>();
        private static Dictionary<int, List<int>> _likes = new Dictionary<int, List<int>>();
        private static int _nextId = 1;

        private static int GetNextId()
        {
            return _nextId++;
        }

        [HttpPost("swipe")]
        public ActionResult<object> Swipe(SwipeRequest request)
        {
            if (!request.UserId.HasValue || !request.TargetUserId.HasValue || !request.IsLike.HasValue)
                return BadRequest("Invalid request data");

            var userId = request.UserId.Value;
            var targetUserId = request.TargetUserId.Value;
            var isLike = request.IsLike.Value;

            if (!_likes.ContainsKey(userId))
                _likes[userId] = new List<int>();

            if (isLike)
            {
                _likes[userId].Add(targetUserId);

                // Check if target user also liked this user
                if (_likes.ContainsKey(targetUserId) && 
                    _likes[targetUserId].Contains(userId))
                {
                    var match = new Match
                    {
                        Id = GetNextId(),
                        UserId1 = userId,
                        UserId2 = targetUserId
                    };
                    _matches.Add(match);
                    return Ok(new { IsMatch = true, Match = match });
                }
            }

            return Ok(new { IsMatch = false });
        }

        [HttpGet("potential/{userId}")]
        public ActionResult<IEnumerable<UserProfile>> GetPotentialMatches(int userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound();

            var alreadyLiked = _likes.ContainsKey(userId) ? _likes[userId] : new List<int>();
            var matchedUsers = _matches.Where(m => m.UserId1 == userId || m.UserId2 == userId)
                                    .SelectMany(m => new[] { m.UserId1, m.UserId2 })
                                    .Where(id => id != userId);

            var potentialMatches = _users.Where(u => 
                u.Id != userId && 
                u.Gender == user.InterestedIn &&
                u.InterestedIn == user.Gender &&
                !alreadyLiked.Contains(u.Id) &&
                !matchedUsers.Contains(u.Id)
            ).Take(10);

            return Ok(potentialMatches);
        }

        [HttpGet("matches/{userId}")]
        public ActionResult<IEnumerable<object>> GetUserMatches(int userId)
        {
            var userMatches = _matches.Where(m => 
                (m.UserId1 == userId || m.UserId2 == userId) && m.IsActive)
                .Select(m => new
                {
                    MatchId = m.Id,
                    MatchedUser = _users.FirstOrDefault(u => u.Id == (m.UserId1 == userId ? m.UserId2 : m.UserId1)),
                    MatchedAt = m.MatchedAt
                });

            return Ok(userMatches);
        }

        [HttpPost("sync-users")]
        public IActionResult SyncUsers(List<UserProfile> users)
        {
            SetUsers(users);
            return Ok();
        }

        private static void SetUsers(List<UserProfile> users)
        {
            _users = users;
        }
    }
}