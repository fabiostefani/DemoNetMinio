using DemoNetMinio.Storage.Abstractions;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;

namespace DemoNetMinio.Storage;

public class StorageService : IStorageService
{
    public async Task UploadAsync()
    {
        var endpoint = "minio:9000";
        var accessKey = "fabio";
        var secretKey = "fabio123";
        var secure = false;
        
        
        var bucketName = "mybucket";
        var location   = "us-east-1";
        var objectName = "boleto-Outubro-2022.pdf";
        var filePath = @"C:\\boleto-Outubro-2022.pdf";
        var contentType = "application/pdf";
        
        try
        {
            var minio = new MinioClient()
                .WithEndpoint("localhost", 9000)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(secure)
                .Build();
            
            // Make a bucket on the server, if not already present.
            var beArgs = new BucketExistsArgs().WithBucket(bucketName);
            
            bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs().WithBucket(bucketName);
                await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
            }
            // Upload a file to bucket.
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithFileName(filePath)
                .WithContentType(contentType);
            await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
            Console.WriteLine("Successfully uploaded " + objectName );
        }
        catch (MinioException e)
        {
            Console.WriteLine("File Upload Error: {0}", e.Message);
        }
    }

    public async Task RemoveAsync()
    {
        var endpoint = "minio:9000";
        var accessKey = "fabio";
        var secretKey = "fabio123";
        var secure = false;
        
        
        var bucketName = "mybucket";
        var location   = "us-east-1";
        var objectName = "boleto-Outubro-2022.pdf";
        var filePath = @"C:\\boleto-Outubro-2022.pdf";
        var contentType = "application/pdf";
        
        try
        {
            var minio = new MinioClient()
                .WithEndpoint("localhost", 9000)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(secure)
                .Build();
            
            // Make a bucket on the server, if not already present.
            var beArgs = new BucketExistsArgs().WithBucket(bucketName);
            
            bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs().WithBucket(bucketName);
                await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
            }
            // Upload a file to bucket.
            var putObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName);
                //.WithFileName(filePath)
                //.WithContentType(contentType);
            
            await minio.RemoveObjectAsync(putObjectArgs).ConfigureAwait(false);
            Console.WriteLine("Successfully uploaded " + objectName );
        }
        catch (MinioException e)
        {
            Console.WriteLine("Delete File Error: {0}", e.Message);
        }
    }
}