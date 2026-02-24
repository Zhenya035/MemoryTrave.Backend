using MemoryTrave.Application.Dto.Responses.Article;

namespace MemoryTrave.Application.Dto.Responses.User;

public class GetProfileDto
{
    public string Username  { get; set; }
    public string Email  { get; set; }
    public string Role  { get; set; }
    public int FriendsCount { get; set; }
    public int ArticlesCount { get; set; }
    public List<GetArticleForProfileDto> Articles { get; set; }
}