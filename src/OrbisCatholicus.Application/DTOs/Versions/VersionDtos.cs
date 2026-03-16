namespace OrbisCatholicus.Application.DTOs;

public record ArticleVersionListDto(
    int Id,
    int VersionNumber,
    string? EditSummary,
    string AuthorName,
    string? ReviewStatusName,
    string? ReviewStatusColor,
    DateTime CreatedAt
);

public record CreateVersionDto(
    string Content,
    string ContentHtml,
    string? EditSummary
);

public record ReviewVersionDto(
    int NewStatusId,
    string? Notes
);
