namespace PhotoService.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool IsMain { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }

    public class UploadPhotoRequest
    {
        public string UserId { get; set; } = string.Empty;
        public required IFormFile File { get; set; }
        public bool? IsMain { get; set; }
    }
}