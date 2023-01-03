using System.Reactive.Linq;
using DemoNetMinio.Storage.Abstractions;
using Minio;
using Minio.DataModel;

namespace DemoNetMinio.Storage;

public class BucketStorageService : IBucketStorageService
{
    private readonly MinioClient _minioClient;
    public BucketStorageService(MinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<ListAllMyBucketsResult> ListBucketsAsync()
    {
        return await _minioClient.ListBucketsAsync();
    }

    public async Task CreateBucketAsync(string nameBucket)
    {
        var makeBucketArgs = new MakeBucketArgs().WithBucket(nameBucket);
        await _minioClient.MakeBucketAsync(makeBucketArgs);
    }

    public async Task<bool> BucketExistsAsync(string nameBucket)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(nameBucket);
        return await _minioClient.BucketExistsAsync(bucketExistsArgs);
    }

    public async Task RemoveBucketAsync(string nameBucket)
    {
        var existFileBucket = await _minioClient.ListObjectsAsync(new ListObjectsArgs().WithBucket(nameBucket)).Any();
        if (existFileBucket) throw new Exception($"Não é possível excluir o Bucket {nameBucket}. Existem arquivos adicionados.");
        var removeBucketArgs = new RemoveBucketArgs().WithBucket(nameBucket);
        await _minioClient.RemoveBucketAsync(removeBucketArgs);
    }
}