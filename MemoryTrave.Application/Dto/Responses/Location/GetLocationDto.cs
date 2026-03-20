using MemoryTrave.Application.Dto.Responses.Article;

namespace MemoryTrave.Application.Dto.Responses.Location;

public class GetLocationDto
{
    public string Name { get; set; } = string.Empty;
    public List<GetArticleDto> Articles { get; set; } = [];
}