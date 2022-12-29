using DemoNetMinio.Storage.Abstractions;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;

namespace DemoNetMinio.Storage;

public class BucketStorageService : IBucketStorageService
{
    private readonly StorageOptions _storageOptions;

    public BucketStorageService(IOptions<StorageOptions> options)
    {
        _storageOptions = options.Value;
    }

    public async Task<ListAllMyBucketsResult> ListBucketsAsync()
    {
        var endpoint = "localhost";
        var accessKey = "fabio";
        var secretKey = "fabio123";
        var secure = false;


        var minio = new MinioClient()
            .WithEndpoint(_storageOptions.HostStorage, _storageOptions.PortStorage)
            .WithCredentials(_storageOptions.AccessKeyStorage, _storageOptions.SecretKeyStorage)
            .WithSSL(secure)
            .Build();

        return await minio.ListBucketsAsync();
    }
}