namespace DemoNetMinio.Storage;

public class StorageOptions
{
    public string HostStorage { get; set; } = string.Empty;
    public int PortStorage { get; set; }
    public string AccessKeyStorage { get; set; } = string.Empty;
    public string SecretKeyStorage { get; set; } = string.Empty;
}