using AutoMapper;
using OrbisCatholicus.Domain.Entities;
using OrbisCatholicus.Application.DTOs;

namespace OrbisCatholicus.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Article -> ArticleListDto
        CreateMap<Article, ArticleListDto>()
            .ForCtorParam("CategoryName", opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForCtorParam("CategorySlug", opt => opt.MapFrom(src => src.Category != null ? src.Category.Slug : null))
            .ForCtorParam("CategoryIcon", opt => opt.MapFrom(src => src.Category != null ? src.Category.IconClass : null))
            .ForCtorParam("AuthorName", opt => opt.MapFrom(src => src.Author.DisplayName))
            .ForCtorParam("AuthorUsername", opt => opt.MapFrom(src => src.Author.Username))
            .ForCtorParam("ReviewStatusName", opt => opt.MapFrom(src => src.CurrentVersion != null && src.CurrentVersion.ReviewStatus != null ? src.CurrentVersion.ReviewStatus.Name : null))
            .ForCtorParam("ReviewStatusColor", opt => opt.MapFrom(src => src.CurrentVersion != null && src.CurrentVersion.ReviewStatus != null ? src.CurrentVersion.ReviewStatus.ColorHex : null));

        // Article -> ArticleDetailDto
        CreateMap<Article, ArticleDetailDto>()
            .ForCtorParam("CategoryName", opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForCtorParam("CategorySlug", opt => opt.MapFrom(src => src.Category != null ? src.Category.Slug : null))
            .ForCtorParam("CategoryIcon", opt => opt.MapFrom(src => src.Category != null ? src.Category.IconClass : null))
            .ForCtorParam("AuthorName", opt => opt.MapFrom(src => src.Author.DisplayName))
            .ForCtorParam("AuthorUsername", opt => opt.MapFrom(src => src.Author.Username))
            .ForCtorParam("VersionNumber", opt => opt.MapFrom(src => src.CurrentVersion != null ? src.CurrentVersion.VersionNumber : (int?)null))
            .ForCtorParam("ContentHtml", opt => opt.MapFrom(src => src.CurrentVersion != null ? src.CurrentVersion.ContentHtml : null))
            .ForCtorParam("ReviewStatusName", opt => opt.MapFrom(src => src.CurrentVersion != null && src.CurrentVersion.ReviewStatus != null ? src.CurrentVersion.ReviewStatus.Name : null))
            .ForCtorParam("ReviewStatusColor", opt => opt.MapFrom(src => src.CurrentVersion != null && src.CurrentVersion.ReviewStatus != null ? src.CurrentVersion.ReviewStatus.ColorHex : null))
            .ForCtorParam("Tags", opt => opt.MapFrom(src => src.Tags.Select(t => t.Tag)))
            .ForCtorParam("References", opt => opt.MapFrom(src => src.References))
            .ForCtorParam("RelatedArticles", opt => opt.MapFrom(src => src.RelatedArticles));

        // ArticleReference -> ArticleReferenceDto
        CreateMap<ArticleReference, ArticleReferenceDto>();

        // RelatedArticle -> RelatedArticleDto
        CreateMap<RelatedArticle, RelatedArticleDto>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.Related.Id))
            .ForCtorParam("Title", opt => opt.MapFrom(src => src.Related.Title))
            .ForCtorParam("Slug", opt => opt.MapFrom(src => src.Related.Slug))
            .ForCtorParam("RelationType", opt => opt.MapFrom(src => src.RelationType));

        // ArticleVersion -> ArticleVersionListDto
        CreateMap<ArticleVersion, ArticleVersionListDto>()
            .ForCtorParam("AuthorName", opt => opt.MapFrom(src => src.Author.DisplayName))
            .ForCtorParam("ReviewStatusName", opt => opt.MapFrom(src => src.ReviewStatus != null ? src.ReviewStatus.Name : null))
            .ForCtorParam("ReviewStatusColor", opt => opt.MapFrom(src => src.ReviewStatus != null ? src.ReviewStatus.ColorHex : null));

        // Category mappings
        CreateMap<Category, CategoryListDto>()
            .ForCtorParam("ArticleCount", opt => opt.MapFrom(src => src.Articles != null ? src.Articles.Count : 0));

        CreateMap<Category, CategoryDetailDto>()
            .ForCtorParam("SubCategories", opt => opt.MapFrom(src => src.SubCategories));

        // User -> UserProfileDto
        CreateMap<User, UserProfileDto>()
            .ForCtorParam("AccessLevelName", opt => opt.MapFrom(src => src.AccessLevel.Name));
    }
}
