using AutoMapper;
using OrbisCatholicus.Application.Common;
using OrbisCatholicus.Application.DTOs;
using OrbisCatholicus.Application.Interfaces;
using OrbisCatholicus.Domain.Entities;
using OrbisCatholicus.Domain.Enums;
using OrbisCatholicus.Domain.Interfaces;

namespace OrbisCatholicus.Application.Services;

public class ArticleVersionService : IArticleVersionService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ArticleVersionService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ArticleVersionListDto>>> GetByArticleIdAsync(int articleId)
    {
        var versions = await _uow.ArticleVersions.GetByArticleIdAsync(articleId);
        return Result<IEnumerable<ArticleVersionListDto>>.Ok(
            _mapper.Map<IEnumerable<ArticleVersionListDto>>(versions));
    }

    public async Task<Result<ArticleVersionListDto>> CreateVersionAsync(int articleId, CreateVersionDto dto, int userId)
    {
        var article = await _uow.Articles.GetByIdAsync(articleId);
        if (article == null)
            return Result<ArticleVersionListDto>.Fail("Artigo não encontrado.");

        var nextVersion = await _uow.ArticleVersions.GetNextVersionNumberAsync(articleId);

        var version = new ArticleVersion
        {
            ArticleId = articleId,
            VersionNumber = nextVersion,
            Content = dto.Content,
            ContentHtml = dto.ContentHtml,
            EditSummary = dto.EditSummary,
            CreatedBy = userId,
            ReviewStatusId = (int)ReviewStatusEnum.Pendente
        };

        await _uow.ArticleVersions.AddAsync(version);
        await _uow.SaveChangesAsync();

        // Reload with includes
        var versions = await _uow.ArticleVersions.GetByArticleIdAsync(articleId);
        var created = versions.First(v => v.Id == version.Id);

        return Result<ArticleVersionListDto>.Ok(_mapper.Map<ArticleVersionListDto>(created));
    }

    public async Task<Result> ReviewAsync(int versionId, ReviewVersionDto dto, int reviewerId)
    {
        var version = await _uow.ArticleVersions.GetByIdAsync(versionId);
        if (version == null)
            return Result.Fail("Versão não encontrada.");

        var validStatuses = new[] {
            (int)ReviewStatusEnum.Aprovado,
            (int)ReviewStatusEnum.Rejeitado,
            (int)ReviewStatusEnum.EmRevisao
        };

        if (!validStatuses.Contains(dto.NewStatusId))
            return Result.Fail("Status de revisão inválido.");

        var previousStatusId = version.ReviewStatusId;

        version.ReviewStatusId = dto.NewStatusId;
        version.ReviewedBy = reviewerId;
        version.ReviewedAt = DateTime.UtcNow;
        version.Notes = dto.Notes;

        _uow.ArticleVersions.Update(version);

        // Add review history
        await _uow.Repository<ArticleReviewHistory>().AddAsync(new ArticleReviewHistory
        {
            ArticleVersionId = versionId,
            ReviewerId = reviewerId,
            PreviousStatusId = previousStatusId,
            NewStatusId = dto.NewStatusId,
            Notes = dto.Notes,
            ReviewedAt = DateTime.UtcNow
        });

        // If approved, update current version of article
        if (dto.NewStatusId == (int)ReviewStatusEnum.Aprovado)
        {
            var article = await _uow.Articles.GetByIdAsync(version.ArticleId);
            if (article != null)
            {
                article.CurrentVersionId = versionId;
                _uow.Articles.Update(article);
            }
        }

        await _uow.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result<IEnumerable<ArticleVersionListDto>>> GetPendingReviewsAsync(int page = 1, int pageSize = 20)
    {
        var versions = await _uow.ArticleVersions.GetPendingReviewsAsync(page, pageSize);
        return Result<IEnumerable<ArticleVersionListDto>>.Ok(
            _mapper.Map<IEnumerable<ArticleVersionListDto>>(versions));
    }
}
