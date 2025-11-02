using Microsoft.AspNetCore.Mvc;
using PhotoService.Models;

namespace PhotoService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {
        private static List<Photo> _photos = new List<Photo>();
        private static int _nextId = 1;

        private static int GetNextId()
        {
            return _nextId++;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<Photo>> UploadPhoto([FromForm] UploadPhotoRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded");

            var fileName = $"{Guid.NewGuid()}_{request.File.FileName}";
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            var photo = new Photo
            {
                Id = GetNextId(),
                UserId = request.UserId,
                Url = $"/uploads/{fileName}",
                IsMain = request.IsMain ?? false
            };

            _photos.Add(photo);
            return CreatedAtAction(nameof(GetPhoto), new { id = photo.Id }, photo);
        }

        [HttpGet("{id}")]
        public ActionResult<Photo> GetPhoto(int id)
        {
            var photo = _photos.FirstOrDefault(p => p.Id == id);
            if (photo == null)
                return NotFound();
            return photo;
        }

        [HttpGet("user/{userId}")]
        public ActionResult<IEnumerable<Photo>> GetUserPhotos(string userId)
        {
            return _photos.Where(p => p.UserId == userId).ToList();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePhoto(int id)
        {
            var photo = _photos.FirstOrDefault(p => p.Id == id);
            if (photo == null)
                return NotFound();

            _photos.Remove(photo);
            
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", Path.GetFileName(photo.Url));
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            return NoContent();
        }
    }
}