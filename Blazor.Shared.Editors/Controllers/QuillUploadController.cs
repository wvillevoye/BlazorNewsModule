using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Blazor.Shared.Editors.Controllers
{
    [ApiController]
    [Route("api/quill")]
    public class QuillUploadController(IWebHostEnvironment env) : Controller
    {
        private readonly IWebHostEnvironment _Env = env;

        [HttpPost("upload")]
        [RequestSizeLimit(1 * 1024 * 1024)] // 1 MB
        public async Task<IActionResult> UploadImage(IFormFile image, [FromQuery] int? userId = null)
        {
            const long maxFileSize = 1 * 1024 * 1024;

            if (image == null || image.Length == 0)
                return BadRequest(new { error = "No image uploaded" });

            if (image.Length > maxFileSize)
                return BadRequest(new { error = "Bestand is te groot. Maximaal 1MB toegestaan." });

            using var img = await Image.LoadAsync(image.OpenReadStream());

            if (img.Width > 800)
            {
                var newHeight = (int)(img.Height * (800.0 / img.Width));
                img.Mutate(x => x.Resize(800, newHeight));
            }

            var uploadsFolder = Path.Combine(_Env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Bestandsnaam opbouwen
            var originalBase = Slugify(Path.GetFileNameWithoutExtension(image.FileName));
            if (originalBase.Length > 8)
                originalBase = originalBase.Substring(0, 8);

            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

            var userPart = userId.HasValue ? $"_uid{userId}" : "";

            var finalName = $"{originalBase}_{timestamp}{userPart}.png";

            var savePath = Path.Combine(uploadsFolder, finalName);

            // Extra zekerheid: als bestand al bestaat (kleine kans), voeg een random suffix toe
            int attempt = 0;
            while (System.IO.File.Exists(savePath) && attempt < 10)
            {
                finalName = $"{originalBase}_{timestamp}{userPart}_{GenerateSafeFileName(4)}.png";
                savePath = Path.Combine(uploadsFolder, finalName);
                attempt++;
            }

            await img.SaveAsync(savePath, new PngEncoder());

            var imageUrl = $"/uploads/{finalName}";
            return Ok(new { url = imageUrl });
        }

        // Slugify de originele naam
        private static string Slugify(string input)
        {
            return new string(input
                .ToLowerInvariant()
                .Replace(" ", "_")
                .Where(c => char.IsLetterOrDigit(c) || c == '_')
                .ToArray());
        }

        // Random fallback component
        private static string GenerateSafeFileName(int length = 4)
        {
            var random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }






        [HttpGet("images")]
        public IActionResult GetImages()
        {
            var uploadsFolder = Path.Combine(_Env.WebRootPath, "uploads");

            if (!Directory.Exists(uploadsFolder))
                return Ok(Array.Empty<string>());

            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".webp", ".gif" };

            var images = Directory.GetFiles(uploadsFolder)
                  .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                  .Select(file => new ImageFile
                  {
                      Url = "/uploads/" + Path.GetFileName(file),
                      FileName = Path.GetFileName(file),
                      Size = new FileInfo(file).Length,
                      LastModified = System.IO.File.GetLastWriteTime(file)
                  })
                  .ToList();

            return Ok(images);
        }

        [HttpDelete("images/{fileName}")]
        public IActionResult DeleteImage(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest("Bestandsnaam is ongeldig.");

            // Beveiliging: voorkom path traversal
            fileName = Path.GetFileName(fileName);

            var uploadsFolder = Path.Combine(_Env.WebRootPath, "uploads");
            var fullPath = Path.Combine(uploadsFolder, fileName);

            if (!System.IO.File.Exists(fullPath))
                return NotFound("Afbeelding niet gevonden.");

            try
            {
                System.IO.File.Delete(fullPath);
                return NoContent(); // 204 OK zonder body
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fout bij verwijderen: {ex.Message}");
            }
        }
    }
}
