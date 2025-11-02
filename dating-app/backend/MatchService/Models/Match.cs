namespace MatchService.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public DateTime MatchedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }

    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Bio { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string InterestedIn { get; set; } = string.Empty;
        public List<string> Photos { get; set; } = new List<string>();
    }

    public class SwipeRequest
    {
        public int UserId { get; set; }
        public int TargetUserId { get; set; }
        public bool IsLike { get; set; }
    }
}