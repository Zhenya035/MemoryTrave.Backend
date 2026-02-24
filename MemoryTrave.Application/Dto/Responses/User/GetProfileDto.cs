using MemoryTrave.Application.Dto.Responses.Article;

namespace MemoryTrave.Application.Dto.Responses.User;

public class GetProfileDto
{
    public string Username  { get; set; } = string.Empty;
    public string Email  { get; set; } = string.Empty;
    public string Role  { get; set; } = string.Empty;
    public int FriendsCount { get; set; }
    public int ArticlesCount { get; set; }
    public List<GetArticleForProfileDto> Articles { get; set; } = [];
}