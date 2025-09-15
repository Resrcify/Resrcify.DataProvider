namespace Resrcify.DataProvider.Infrastructure.HttpClients;

internal class MetadataRequest
{
    public ClientSpecs? ClientSpecs { get; set; }

    public MetadataRequestType RequestType { get; set; }
}
internal class ClientSpecs
{
    public string? GlExtensionData { get; set; }

    public string? Platform { get; set; }

    public string? DeviceModel { get; set; }

    public string? OperatingSystem { get; set; }

    public int SystemMemorySize { get; set; }

    public int GraphicsMemorySize { get; set; }

    public string? GraphicsDeviceName { get; set; }

    public string? GraphicsDeviceVendor { get; set; }

    public string? GraphicsDeviceVersion { get; set; }

    public string? ProcessorType { get; set; }

    public int ProcessorCount { get; set; }

    public string? BundleId { get; set; }

    public string? ExternalVersion { get; set; }

    public string? InternalVersion { get; set; }

    public string? Region { get; set; }
}
internal enum MetadataRequestType
{
    MetadataRequestTypeDEFAULT,
    Default,
    Clientparams
}