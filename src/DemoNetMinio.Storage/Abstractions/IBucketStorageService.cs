using Minio.DataModel;

namespace DemoNetMinio.Storage.Abstractions;

public interface IBucketStorageService
{
    Task<ListAllMyBucketsResult> ListBucketsAsync();
}