using OrbisCatholicus.Application.Common;
using OrbisCatholicus.Application.DTOs;

namespace OrbisCatholicus.Application.Interfaces;

public interface IPortalService
{
    Task<Result<PortalStatsDto>> GetStatsAsync();
}
