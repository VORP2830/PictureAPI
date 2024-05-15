using Microsoft.AspNetCore.Mvc;

namespace Picture.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PictureController : ControllerBase
{
    private string AssetsFolder;
    public PictureController()
    {
        string envVariable = Environment.GetEnvironmentVariable("RAILWAY_VOLUME_MOUNT_PATH");
        AssetsFolder = !string.IsNullOrEmpty(envVariable) ? envVariable : "assets";

        if (!Directory.Exists(AssetsFolder))
        {
            Directory.CreateDirectory(AssetsFolder);
        }
    }
    [HttpGet]
    public IActionResult GetPictureNames()
    {
        string[] fileNames = Directory.GetFiles(AssetsFolder);
        List<string> names = new List<string>();
        foreach (string fileName in fileNames)
        {
            names.Add(Path.GetFileName(fileName));
        }
        return Ok(names);
    }
    [HttpGet("{fileName}")]
    public IActionResult GetPicture(string fileName)
    {
        string filePath = Path.Combine(AssetsFolder, fileName);
        if (System.IO.File.Exists(filePath))
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/jpeg");
        }
        else
        {
            return NotFound();
        }
    }
    [HttpPost]
    public IActionResult UploadPicture(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Nenhum arquivo enviado.");
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(AssetsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok("Upload bem sucedido.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao fazer upload: {ex.Message}");
        }
    }
}
