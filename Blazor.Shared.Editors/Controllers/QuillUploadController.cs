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
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            const long maxFileSize = 1 * 1024 * 1024; // 1MB

            if (image == null || image.Length == 0)
                return BadRequest(new { error = "No image uploaded" });

            if (image.Length > maxFileSize)
                return BadRequest(new { error = "Bestand is te groot. Maximaal 1MB toegestaan." });


            // Open image met ImageSharp
            using var img = await Image.LoadAsync(image.OpenReadStream());

            // Bepaal nieuwe breedte als groter dan 800
            if (img.Width > 800)
            {
                var newHeight = (int)(img.Height * (800.0 / img.Width));
                img.Mutate(x => x.Resize(800, newHeight));
            }
             
            // Definieer het pad naar de 'uploads' map
            var uploadsFolder = Path.Combine(_Env.WebRootPath, "uploads");

            // Controleer of de map bestaat, zo niet, maak hem aan
            if (!Directory.Exists(uploadsFolder))
            {
                //Console.WriteLine($"Aanmaken van upload map: {uploadsFolder}"); // Debugging
                Directory.CreateDirectory(uploadsFolder);
            }
 
            var fileName = Path.GetRandomFileName() + ".png";  // Voor consistentie altijd png
            var savePath = Path.Combine(_Env.WebRootPath, "uploads", fileName);

            // Opslaan als PNG
            await img.SaveAsync(savePath, new PngEncoder());

            var imageUrl = $"/uploads/{fileName}";
            return Ok(new { url = imageUrl });
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
