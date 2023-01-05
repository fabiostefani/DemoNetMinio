using System.Collections;
using DemoNetMinio.Storage.Abstractions;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
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
    public async Task UploadAsync( string fileName, byte[] bytesFile)
    {
        var contentType = "application/octet-stream";
        
        try
        {
            await CreateBucketIfNotExists(_options.DefaultBucket);
            using (var fileStream = new MemoryStream(bytesFile))
            {
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(_options.DefaultBucket)
                    .WithObject(fileName)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileStream.Length)
                    .WithContentType(contentType);
                await _minioClient.PutObjectAsync(putObjectArgs);
            }
            Console.WriteLine("Successfully uploaded " + fileName );
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

    public async Task RemoveAsync(string objectName)
    {
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

    public async Task<ObjectStat> ObjectStatus(string objectName)
    {
        var statObjectArgs = new StatObjectArgs()
            .WithBucket(_options.DefaultBucket)
            .WithObject(objectName);
        return await _minioClient.StatObjectAsync(statObjectArgs);
    }

    public void RemoveAllFromBucket()
    {
        var docs = _minioClient.ListObjectsAsync(new ListObjectsArgs().WithBucket(_options.DefaultBucket).WithRecursive(true));
        IDisposable subscription = docs.Subscribe(
            // async item => Console.WriteLine("OnNext: {0}", item.Key),
            async item => await RemoveAsync(item.Key),
            ex => Console.WriteLine("OnError: {0}", ex.Message),
            () => Console.WriteLine("OnComplete: {0}")
        );
    }

    public async Task<MemoryStream> GetObject(string objectName)
    {
        var memoryStream = new MemoryStream();
        var args = new GetObjectArgs()
            .WithBucket(_options.DefaultBucket)
            .WithObject(objectName)
            .WithCallbackStream((stream) =>
            {
                stream.CopyTo(memoryStream);
            });
        await _minioClient.GetObjectAsync(args);
        return memoryStream;
    }

    public async Task<string> UploadPresignedObjectAsync(string fileName, byte[] bytesFile)
    {
        await UploadAsync(fileName, bytesFile);
        var getObjectArgs = new PresignedGetObjectArgs()
            .WithBucket(_options.DefaultBucket)
            .WithObject(fileName)
            .WithExpiry(20);
        var url = await _minioClient.PresignedGetObjectAsync(getObjectArgs);
        return url;
    }
}