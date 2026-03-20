using Amazon.S3;

namespace MemoryTrave.Domain.Interfaces;

public interface IR2Service
{
    IAmazonS3 CreateClient();
}