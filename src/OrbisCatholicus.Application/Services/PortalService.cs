using OrbisCatholicus.Application.Common;
using OrbisCatholicus.Application.DTOs;
using OrbisCatholicus.Application.Interfaces;
using OrbisCatholicus.Domain.Enums;
using OrbisCatholicus.Domain.Interfaces;

namespace OrbisCatholicus.Application.Services;

public class PortalService : IPortalService
{
    private readonly IUnitOfWork _uow;

    public PortalService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<PortalStatsDto>> GetStatsAsync()
    {
        var totalArticles = await _uow.Articles.CountAsync();
        var totalUsers = await _uow.Users.CountAsync();
        var totalCategories = await _uow.Categories.CountAsync();

        var allArticles = await _uow.Articles.GetAllAsync();
        var totalViews = allArticles.Sum(a => (long)a.Views);

        var pendingReviews = await _uow.ArticleVersions.CountAsync(
            v => v.ReviewStatusId == (int)ReviewStatusEnum.Pendente);

        return Result<PortalStatsDto>.Ok(new PortalStatsDto(
            TotalArticles: totalArticles,
            TotalUsers: totalUsers,
            TotalCategories: totalCategories,
            TotalViews: totalViews,
            PendingReviews: pendingReviews
        ));
    }
}
