using Minio.DataModel;

namespace DemoNetMinio.Storage.Abstractions;

public interface IBucketStorageService
{
    Task<ListAllMyBucketsResult> ListBucketsAsync();
    Task CreateBucketAsync(string name);
    Task<bool> BucketExistsAsync(string nameBucket);
    Task RemoveBucketAsync(string nameBucket);
}