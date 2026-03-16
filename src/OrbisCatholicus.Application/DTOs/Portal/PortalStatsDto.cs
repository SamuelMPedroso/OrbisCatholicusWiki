namespace OrbisCatholicus.Application.DTOs;

public record PortalStatsDto(
    int TotalArticles,
    int TotalUsers,
    int TotalCategories,
    long TotalViews,
    int PendingReviews
);
