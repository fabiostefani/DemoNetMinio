using Minio.DataModel;

namespace DemoNetMinio.Storage.Abstractions;

public interface IStorageService
{
    Task UploadAsync(string fileName, byte[] bytesFile);
    Task RemoveAsync(string objectName);
    Task<ObjectStat> ObjectStatus(string objectName);
}