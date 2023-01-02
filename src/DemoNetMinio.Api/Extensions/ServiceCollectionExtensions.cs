using DemoNetMinio.Storage;
using DemoNetMinio.Storage.IoC;
using Minio;

namespace DemoNetMinio.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        var hostStorage = configuration[EnvironmentWebApiConstants.HostStorage];
        if (string.IsNullOrEmpty(hostStorage)) throw new Exception("Variável HOST_STORAGE não informada.");
         
        var accessKeyStorage = configuration[EnvironmentWebApiConstants.AccessKeyStorage];
        if (string.IsNullOrEmpty(accessKeyStorage)) throw new Exception("Variável ACCESS_KEY_STORAGE não informada.");
        
        var secretKeyStorage = configuration[EnvironmentWebApiConstants.SecretKeyStorage];
        if (string.IsNullOrEmpty(secretKeyStorage)) throw new Exception("Variável SECRET_KEY_STORAGE não informada.");
        
        var portStorage = Convert.ToInt32(configuration[EnvironmentWebApiConstants.PortStorage]);
        
        var defaultBucket = configuration[EnvironmentWebApiConstants.DefaultBucket];
        if (string.IsNullOrEmpty(defaultBucket)) throw new Exception("Variável DEFAULT_BUCKET não informada.");
        
        var defaultLocation = configuration[EnvironmentWebApiConstants.DefaultLocation];
        if (string.IsNullOrEmpty(defaultLocation)) throw new Exception("Variável DEFAULT_LOCATION não informada.");

        services.AddStorageServices(new StorageOptions()
        {
            HostStorage = hostStorage,
            PortStorage = portStorage,
            AccessKeyStorage = accessKeyStorage,
            SecretKeyStorage = secretKeyStorage,
            DefaultBucket = defaultBucket,
            DefaultLocation = defaultLocation
        });
        
        return services;
    }
}