using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            this._fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ??
                throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }
        [HttpGet("{fileId}")]
        public ActionResult GetFile(string fileId)
        {
            var filePath = "creating-the-api-and-returning-resources-slides.pdf";
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var file = System.IO.File.ReadAllBytes(filePath);
            if (!_fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contentType))
                contentType = "application/octet-stream";
 
            return File(file,contentType,Path.GetFileName(filePath));
        }
    }
}
