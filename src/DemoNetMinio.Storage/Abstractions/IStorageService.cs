using Minio.DataModel;

namespace DemoNetMinio.Storage.Abstractions;

public interface IStorageService
{
    Task UploadAsync(string fileName, byte[] bytesFile);
    Task RemoveAsync(string objectName);
    Task<ObjectStat> ObjectStatus(string objectName);
    void RemoveAllFromBucket();
    Task<MemoryStream> GetObject(string objectName);
    Task<string> UploadPresignedObjectAsync(string fileName, byte[] bytesFile);
}