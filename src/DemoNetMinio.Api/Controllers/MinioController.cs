using DemoNetMinio.Storage.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace DemoNetMinio.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MinioController : ControllerBase
{
    
    private readonly ILogger<MinioController> _logger;
    private readonly IStorageService _storageService;

    public MinioController(ILogger<MinioController> logger,
                           IStorageService storageService)
    {
        _logger = logger;
        _storageService = storageService;
    }

    [HttpPost(Name = "post")]
    public async Task<IActionResult> Post(IFormFile fileUpload)
    {
        byte[] content = await ReadBytesFile(fileUpload);
        await _storageService.UploadAsync(fileUpload.FileName, content);
        return Ok();
    }
    
    private static async Task<byte[]> ReadBytesFile(IFormFile formFile)
    {
        using var ms = new MemoryStream();
        await formFile.CopyToAsync(ms);
        var fileBytes = ms.ToArray();
        return fileBytes;
    }
    
    [HttpDelete(Name = "delete")]
    public async Task<IActionResult> Delete(string objectName)
    {
        await _storageService.RemoveAsync(objectName);
        return Ok();
    }

    [HttpGet]
    [Route("file-status/{fileName}")]
    public async Task<IActionResult> GetFileStatus(string fileName)
    {
        return Ok(await _storageService.ObjectStatus(fileName));
    }
}