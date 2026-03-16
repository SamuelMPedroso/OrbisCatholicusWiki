namespace OrbisCatholicus.Application.DTOs;

public record SearchResultDto(
    IEnumerable<ArticleListDto> Items,
    int TotalCount,
    int Page,
    int PageSize,
    string Query
);
