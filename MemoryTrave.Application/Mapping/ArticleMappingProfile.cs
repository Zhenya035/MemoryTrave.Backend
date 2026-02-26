using AutoMapper;
using MemoryTrave.Application.Dto.Responses.Article;
using MemoryTrave.Domain.Enums;
using MemoryTrave.Domain.Models;

namespace MemoryTrave.Application.Mapping;

public class ArticleMappingProfile : Profile
{
    public ArticleMappingProfile()
    {
        CreateMap<Article, GetArticleForProfileDto>()
            .ForMember(dto => dto.LocationName,
                opt => opt.MapFrom(src =>
                    src.Location != null ? src.Location.Name : null))
            .ForMember(dto => dto.IsPrivate,
                opt => opt.MapFrom(src =>
                    src.Visibility == VisibilityEnum.Private))
            .ForMember(dto => dto.EncryptedPreviewData,
                opt => opt.MapFrom(src =>
                    src.Visibility == VisibilityEnum.Private ? src.EncryptedPreviewData : null))
            .ForMember(dto => dto.Description,
                opt => opt.MapFrom(src =>
                    src.Visibility == VisibilityEnum.Public ? src.Description : null));
    }
}