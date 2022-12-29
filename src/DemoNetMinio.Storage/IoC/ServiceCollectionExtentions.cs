using DemoNetMinio.Storage.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DemoNetMinio.Storage.IoC;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddStorageServices(this IServiceCollection services)
    {
        services.AddSingleton<IStorageService, StorageService>();
        services.AddSingleton<IBucketStorageService, BucketStorageService>();
        return services;
    }
}