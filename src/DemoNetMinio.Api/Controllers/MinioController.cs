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
    public async Task<IActionResult> Post()
    {
        await _storageService.UploadAsync();
        return Ok();
    }
}