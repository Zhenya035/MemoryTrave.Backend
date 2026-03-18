using AutoMapper;
using MemoryTrave.Application.Dto.Requests.Article;
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
            .ForMember(dto => dto.Description,
                opt => opt.MapFrom(src =>
                    src.Visibility == VisibilityEnum.Public ? src.Description : null))
            .ForMember(dto => dto.EncryptedDek,
                opt => opt.Ignore());

        CreateMap<Article, GetArticleDto>()
            .ForMember(dto => dto.AuthorName,
                opt => opt.MapFrom(src =>
                    src.Author.Username))
            .ForMember(dto => dto.LocationName,
                opt => opt.MapFrom(src =>
                    src.Location.Name))
            .ForMember(dto => dto.EncryptedKey, 
                opt => opt.Ignore());
        
        CreateMap<AddPublicArticleDto, Article>()
            .ForMember(mod => mod.Id, opt => 
                opt.Ignore())
            .ForMember(mod => mod.Visibility, opt => 
                opt.Ignore())
            .ForMember(mod => mod.CreatedAt, opt => 
                opt.Ignore())
            .ForMember(mod => mod.LastChange, opt => 
                opt.Ignore())
            .ForMember(mod => mod.EncryptedKeys, opt => 
                opt.Ignore())
            .ForMember(mod => mod.AuthorId, opt =>
                opt.Ignore());
        
        CreateMap<UpdateArticleDto, Article>()
            .ForMember(mod => mod.Id, opt => 
                opt.Ignore())
            .ForMember(mod => mod.CreatedAt, opt => 
                opt.Ignore())
            .ForMember(mod => mod.EncryptedKeys, opt => 
                opt.Ignore())
            .ForMember(mod => mod.AuthorId, opt =>
                opt.Ignore())
            .ForMember(mod => mod.LocationId, opt =>
                opt.Ignore());
    }
}