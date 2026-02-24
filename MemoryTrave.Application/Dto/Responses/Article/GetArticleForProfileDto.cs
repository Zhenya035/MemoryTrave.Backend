namespace MemoryTrave.Application.Dto.Responses.Article;

public class GetArticleForProfileDto
{
    public Guid Id { get; set; }
    public string? LocationName { get; set; } = string.Empty;
    public DateTime LastChange { get; set; }
    public bool IsPrivate { get; set; }
    public string? EncryptedPreviewData  { get; set; }
    public string? Description { get; set; }
}