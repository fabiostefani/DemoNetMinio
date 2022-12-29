using DemoNetMinio.Storage;
using DemoNetMinio.Storage.IoC;

namespace DemoNetMinio.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStorageServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StorageOptions>(options =>
        {
            var hostStorage = configuration[EnvironmentWebApiConstants.HostStorage];
            if (string.IsNullOrEmpty(hostStorage)) throw new Exception("Variável HOST_STORAGE não informada.");
            
            var accessKeyStorage = configuration[EnvironmentWebApiConstants.AccessKeyStorage];
            if (string.IsNullOrEmpty(accessKeyStorage)) throw new Exception("Variável ACCESS_KEY_STORAGE não informada.");
            
            var secretKeyStorage = configuration[EnvironmentWebApiConstants.SecretKeyStorage];
            if (string.IsNullOrEmpty(secretKeyStorage)) throw new Exception("Variável SECRET_KEY_STORAGE não informada.");
            var portStorage = Convert.ToInt32(configuration[EnvironmentWebApiConstants.PortStorage]);
            //if (string.IsNullOrEmpty(portStorage)) throw new Exception("Variável PORT_STORAGE não informada.")
            
            options.HostStorage = hostStorage;
            options.AccessKeyStorage = accessKeyStorage;
            options.SecretKeyStorage = secretKeyStorage;
            options.PortStorage = portStorage;
        });
        services.AddStorageServices();
        return services;
    }
}