using MemoryTrave.Application.Dto.Responses.Article.GetArticle;

namespace MemoryTrave.Application.Dto.Responses.Location;

public class GetLocationDto
{
    public string Name { get; set; } = string.Empty;
    public List<GetArticleBaseDto> Articles { get; set; } = [];
}