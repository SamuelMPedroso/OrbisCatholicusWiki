namespace OrbisCatholicus.Application.DTOs;

public record CategoryListDto(
    int Id,
    string Name,
    string Slug,
    string? Description,
    string? IconClass,
    int DisplayOrder,
    int ArticleCount
);

public record CategoryDetailDto(
    int Id,
    string Name,
    string Slug,
    string? Description,
    string? IconClass,
    IEnumerable<CategoryListDto> SubCategories
);
