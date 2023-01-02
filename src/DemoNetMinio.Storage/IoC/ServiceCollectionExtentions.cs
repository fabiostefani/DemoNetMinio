using DemoNetMinio.Storage.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace DemoNetMinio.Storage.IoC;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddStorageServices(this IServiceCollection services, StorageOptions options)
    {
        services.Configure<StorageOptions>(storageOptions =>
        {
            storageOptions.HostStorage = options.HostStorage;
            storageOptions.AccessKeyStorage = options.AccessKeyStorage;
            storageOptions.SecretKeyStorage = options.SecretKeyStorage;
            storageOptions.PortStorage = options.PortStorage;
            storageOptions.DefaultLocation = options.DefaultLocation;
            storageOptions.DefaultBucket = options.DefaultBucket;
        });
        
        services.AddSingleton(sp =>
        {
            var minio = new MinioClient()
                .WithEndpoint(options.HostStorage, options.PortStorage)
                .WithCredentials(options.AccessKeyStorage, options.SecretKeyStorage)
                .WithSSL(false)
                .WithRegion(options.DefaultLocation)
                .Build();
            return minio;
        });
        
        services.AddSingleton<IStorageService, StorageService>();
        services.AddSingleton<IBucketStorageService, BucketStorageService>();

        return services;
    }
}