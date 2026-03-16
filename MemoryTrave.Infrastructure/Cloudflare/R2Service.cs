using Amazon.Runtime;
using Amazon.S3;
using MemoryTrave.Domain.Interfaces;
using Microsoft.Extensions.Options;

namespace MemoryTrave.Infrastructure.Cloudflare;

public class R2Service(IOptions<R2Settings> options) : IR2Service
{
    private readonly R2Settings _options = options.Value;
    
    public IAmazonS3 CreateClient()
    {
        var credentials = new BasicAWSCredentials(_options.AccessKeyId, _options.SecretAccessKey);
        
        var config = new AmazonS3Config
        {
            ServiceURL = $"https://{_options.AccountId}.r2.cloudflarestorage.com",
            ForcePathStyle = true
        };

        return new AmazonS3Client(credentials, config);
    }
}