using AutoMapper;
using MemoryTrave.Application.Dto.Requests.Article.Access;
using MemoryTrave.Application.Dto.Responses.Article.Access;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Mapping;

public class ArticleAccessMappingProfile : Profile
{
    public ArticleAccessMappingProfile()
    {
        CreateMap<ArticleAccess, GetArticleAccessDto>();
        
        CreateMap<AddAccessDto, ArticleAccess>()
            .ForMember(mod => mod.Id,
                opt => opt.Ignore())
            .ForMember(mod => mod.ArticleId,
                opt => opt.Ignore());
    }
}