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

    public async Task<byte[]> GetObject(string objectName)
    {
        string fileName = "my-file-name.pdf";
        byte[] buffer = new byte[] { };
        var args = new GetObjectArgs()
            .WithBucket(_options.DefaultBucket)
            .WithObject(objectName)
            .WithFile("Fabio.pdf");
            // .WithCallbackStream((stream) =>
            // {
            //     // buffer = new byte[strem.Length];
            //     // strem.Read(buffer, 0, strem.);
            //     // strem.CopyTo(Console.OpenStandardOutput());
            //     var fileStream = File.Create(fileName);
            //     stream.CopyTo(fileStream);
            //     fileStream.Dispose();
            //     var writtenInfo = new FileInfo(fileName);
            //     var file_read_size = writtenInfo.Length;
            //     // Uncomment to print the file on output console
            //     // stream.CopyTo(Console.OpenStandardOutput());
            //     Console.WriteLine(
            //         $"Successfully downloaded object with requested offset and length {writtenInfo.Length} into file");
            //     stream.Dispose();
            // });
        var document = await _minioClient.GetObjectAsync(args);
        return buffer;
    }
    
    
}