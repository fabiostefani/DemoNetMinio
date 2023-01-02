using DemoNetMinio.Storage.Abstractions;
using Microsoft.Extensions.Options;
using Minio;
using Minio.Exceptions;

namespace DemoNetMinio.Storage;

public class StorageService : IStorageService
{
    private readonly IBucketStorageService _bucketStorageService;
    private readonly MinioClient _minioClient;
    private readonly StorageOptions _options;

    public StorageService(IBucketStorageService bucketStorageService,
                          MinioClient minioClient,
                          IOptions<StorageOptions> options)
    {
        _bucketStorageService = bucketStorageService;
        _minioClient = minioClient;
        _options = options.Value;
    }
    public async Task UploadAsync()
    {
        var objectName = "boleto-Outubro-2022.pdf";
        var filePath = @"C:\\boleto-Outubro-2022.pdf";
        var contentType = "application/pdf";
        
        try
        {
            await CreateBucketIfNotExists(_options.DefaultBucket);
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_options.DefaultBucket)
                .WithObject(objectName)
                .WithFileName(filePath)
                .WithContentType(contentType);
            await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
            Console.WriteLine("Successfully uploaded " + objectName );
        }
        catch (MinioException e)
        {
            Console.WriteLine("File Upload Error: {0}", e.Message);
        }
    }

    private async Task CreateBucketIfNotExists(string bucketName)
    {
        if (!await _bucketStorageService.BucketExistsAsync(bucketName))
        {
            await _bucketStorageService.CreateBucketAsync(bucketName);
        }
    }

    public async Task RemoveAsync()
    {
        var objectName = "boleto-Outubro-2022.pdf";
        
        try
        {   
            await CreateBucketIfNotExists(_options.DefaultBucket);
            var putObjectArgs = new RemoveObjectArgs()
                .WithBucket(_options.DefaultBucket)
                .WithObject(objectName);

            await _minioClient.RemoveObjectAsync(putObjectArgs).ConfigureAwait(false);
            Console.WriteLine("Successfully Removed " + objectName );
        }
        catch (MinioException e)
        {
            Console.WriteLine("Delete File Error: {0}", e.Message);
        }
    }
}