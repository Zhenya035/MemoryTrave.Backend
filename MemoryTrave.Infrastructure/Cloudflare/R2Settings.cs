namespace MemoryTrave.Infrastructure.Cloudflare;

public class R2Settings
{
    public string AccountId { get; set; } = null!;
    public string AccessKeyId { get; set; } = null!;
    public string SecretAccessKey { get; set; } = null!;
    public string BucketName { get; set; } = null!;
    public string PublicUrlBase { get; set; } = null!;
}