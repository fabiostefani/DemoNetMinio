using DemoNetMinio.Storage.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel;

namespace DemoNetMinio.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BucketMinioController : ControllerBase
{
    
    private readonly ILogger<BucketMinioController> _logger;
    private readonly IBucketStorageService _storageService;

    public BucketMinioController(ILogger<BucketMinioController> logger,
                                 IBucketStorageService storageService)
    {
        _logger = logger;
        _storageService = storageService;
    }

    [HttpGet()]
    public async Task<ListAllMyBucketsResult> Get()
    {
        return await _storageService.ListBucketsAsync();
    }
}