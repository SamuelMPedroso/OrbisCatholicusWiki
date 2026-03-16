namespace OrbisCatholicus.Application.DTOs;

public record ArticleListDto(
    int Id,
    string Title,
    string Slug,
    string? Summary,
    string? FeaturedImageUrl,
    bool IsFeatured,
    int Views,
    DateTime CreatedAt,
    string? CategoryName,
    string? CategorySlug,
    string? CategoryIcon,
    string AuthorName,
    string AuthorUsername,
    string? ReviewStatusName,
    string? ReviewStatusColor
);

public record ArticleDetailDto(
    int Id,
    string Title,
    string Slug,
    string? Summary,
    string? FeaturedImageUrl,
    bool IsFeatured,
    int Views,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    string? CategoryName,
    string? CategorySlug,
    string? CategoryIcon,
    string AuthorName,
    string AuthorUsername,
    int? VersionNumber,
    string? ContentHtml,
    string? ReviewStatusName,
    string? ReviewStatusColor,
    IEnumerable<string> Tags,
    IEnumerable<ArticleReferenceDto> References,
    IEnumerable<RelatedArticleDto> RelatedArticles
);

public record CreateArticleDto(
    string Title,
    string? Summary,
    int? CategoryId,
    string? FeaturedImageUrl,
    string Content,
    string ContentHtml,
    IEnumerable<string>? Tags
);

public record UpdateArticleDto(
    string? Title,
    string? Summary,
    int? CategoryId,
    string? FeaturedImageUrl
);

public record ArticleReferenceDto(
    string ReferenceType,
    string ReferenceText,
    string? ReferenceUrl,
    int DisplayOrder
);

public record RelatedArticleDto(
    int Id,
    string Title,
    string Slug,
    string RelationType
);
