using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProject.Business.Services;
using System.Threading.Tasks;

namespace MyProject.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly FileService _fileService;

        public FileController()
        {
            _fileService = new FileService();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null)
                return BadRequest("No file provided");

            var fileName = await _fileService.SaveFileAsync(file);
            return Ok(new { FileName = fileName });
        }
    }
}