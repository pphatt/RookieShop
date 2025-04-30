using Asp.Versioning;

using HeadphoneStore.API.Filters;
using HeadphoneStore.Application.Abstracts.Interface.Services.Media;
using HeadphoneStore.Shared.Dtos.Media;

using Microsoft.AspNetCore.Mvc;

namespace HeadphoneStore.API.Controllers.V1.Test;


[Tags("Test")]
[ApiVersion(1)]
public class TestMediaController : TestApiController
{
    private readonly ICloudinaryService _cloudinaryService;

    public TestMediaController(ICloudinaryService cloudinaryService)
    {
        _cloudinaryService = cloudinaryService;
    }

    [HttpPost("upload-files")]
    [FileValidationFilter(5 * 1024 * 1024)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UploadFiles([FromForm] List<IFormFile> files, [FromForm] FileRequiredParamsDto request)
    {
        //await _mediaService.SaveFilesAsync(files, type);

        await _cloudinaryService.UploadFilesToCloudinary(files, request);

        return Ok("Save files successfully.");
    }

    [HttpDelete("remove-files")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RemoveFiles(List<DeleteFileDto> request)
    {
        //await _mediaService.RemoveFiles(paths);

        await _cloudinaryService.RemoveFilesFromCloudinary(request);

        return Ok("Delete files successfully.");
    }

    [HttpGet("download-files")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ValidationProblemDetails))]
    [MapToApiVersion(1)]
    public async Task<IActionResult> DownloadFile([FromQuery] List<string> request)
    {
        //try
        //{
        //    var (fileStream, contentType, fileName) = await _mediaService.DownloadFiles(paths);

        //    if (fileStream is MemoryStream memoryStream)
        //    {
        //        return File(memoryStream.ToArray(), contentType, fileName);
        //    }

        //    return File(fileStream, contentType, fileName);
        //}
        //catch (FileNotFoundException ex)
        //{
        //    return NotFound(ex.Message);
        //}
        //catch (Exception ex)
        //{
        //    return StatusCode(500, $"An error occurred: {ex.Message}");
        //}

        var url = _cloudinaryService.GenerateDownloadUrl(request);

        return Ok(url);
    }
}
