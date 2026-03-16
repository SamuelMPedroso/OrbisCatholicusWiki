using System.Text.RegularExpressions;
using AutoMapper;
using OrbisCatholicus.Application.Common;
using OrbisCatholicus.Application.DTOs;
using OrbisCatholicus.Application.Interfaces;
using OrbisCatholicus.Domain.Entities;
using OrbisCatholicus.Domain.Enums;
using OrbisCatholicus.Domain.Interfaces;

namespace OrbisCatholicus.Application.Services;

public partial class ArticleService : IArticleService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ArticleService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<ArticleDetailDto>> GetBySlugAsync(string slug)
    {
        var article = await _uow.Articles.GetBySlugAsync(slug);
        if (article == null)
            return Result<ArticleDetailDto>.Fail("Artigo não encontrado.");

        await _uow.Articles.IncrementViewsAsync(article.Id);

        return Result<ArticleDetailDto>.Ok(_mapper.Map<ArticleDetailDto>(article));
    }

    public async Task<Result<IEnumerable<ArticleListDto>>> GetFeaturedAsync(int count = 5)
    {
        var articles = await _uow.Articles.GetFeaturedAsync(count);
        return Result<IEnumerable<ArticleListDto>>.Ok(_mapper.Map<IEnumerable<ArticleListDto>>(articles));
    }

    public async Task<Result<IEnumerable<ArticleListDto>>> GetRecentAsync(int count = 10)
    {
        var articles = await _uow.Articles.GetRecentAsync(count);
        return Result<IEnumerable<ArticleListDto>>.Ok(_mapper.Map<IEnumerable<ArticleListDto>>(articles));
    }

    public async Task<Result<IEnumerable<ArticleListDto>>> GetPopularAsync(int count = 10)
    {
        var articles = await _uow.Articles.GetPopularAsync(count);
        return Result<IEnumerable<ArticleListDto>>.Ok(_mapper.Map<IEnumerable<ArticleListDto>>(articles));
    }

    public async Task<Result<SearchResultDto>> SearchAsync(string query, int page = 1, int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
            return Result<SearchResultDto>.Fail("Termo de busca não pode ser vazio.");

        var articles = await _uow.Articles.SearchAsync(query, page, pageSize);
        var totalCount = await _uow.Articles.SearchCountAsync(query);

        var result = new SearchResultDto(
            Items: _mapper.Map<IEnumerable<ArticleListDto>>(articles),
            TotalCount: totalCount,
            Page: page,
            PageSize: pageSize,
            Query: query
        );

        return Result<SearchResultDto>.Ok(result);
    }

    public async Task<Result<ArticleDetailDto>> CreateAsync(CreateArticleDto dto, int userId)
    {
        var slug = GenerateSlug(dto.Title);

        if (await _uow.Articles.ExistsAsync(a => a.Slug == slug))
            return Result<ArticleDetailDto>.Fail("Já existe um artigo com título semelhante.");

        var article = new Article
        {
            Title = dto.Title,
            Slug = slug,
            Summary = dto.Summary,
            CategoryId = dto.CategoryId,
            FeaturedImageUrl = dto.FeaturedImageUrl,
            CreatedBy = userId
        };

        await _uow.Articles.AddAsync(article);
        await _uow.SaveChangesAsync();

        // Create first version
        var version = new ArticleVersion
        {
            ArticleId = article.Id,
            VersionNumber = 1,
            Content = dto.Content,
            ContentHtml = dto.ContentHtml,
            EditSummary = "Versão inicial",
            CreatedBy = userId,
            ReviewStatusId = (int)ReviewStatusEnum.Rascunho
        };

        await _uow.ArticleVersions.AddAsync(version);
        await _uow.SaveChangesAsync();

        article.CurrentVersionId = version.Id;
        _uow.Articles.Update(article);

        // Add tags
        if (dto.Tags != null)
        {
            foreach (var tag in dto.Tags.Distinct())
            {
                await _uow.Repository<ArticleTag>().AddAsync(new ArticleTag
                {
                    ArticleId = article.Id,
                    Tag = tag
                });
            }
        }

        await _uow.SaveChangesAsync();

        var created = await _uow.Articles.GetBySlugAsync(slug);
        return Result<ArticleDetailDto>.Ok(_mapper.Map<ArticleDetailDto>(created!));
    }

    public async Task<Result> UpdateAsync(int id, UpdateArticleDto dto, int userId)
    {
        var article = await _uow.Articles.GetByIdAsync(id);
        if (article == null)
            return Result.Fail("Artigo não encontrado.");

        if (dto.Title != null)
        {
            article.Title = dto.Title;
            article.Slug = GenerateSlug(dto.Title);
        }
        if (dto.Summary != null) article.Summary = dto.Summary;
        if (dto.CategoryId.HasValue) article.CategoryId = dto.CategoryId;
        if (dto.FeaturedImageUrl != null) article.FeaturedImageUrl = dto.FeaturedImageUrl;

        _uow.Articles.Update(article);
        await _uow.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeactivateAsync(int id, int userId)
    {
        var article = await _uow.Articles.GetByIdAsync(id);
        if (article == null)
            return Result.Fail("Artigo não encontrado.");

        article.Deactivate();
        _uow.Articles.Update(article);
        await _uow.SaveChangesAsync();
        return Result.Ok();
    }

    private static string GenerateSlug(string title)
    {
        var slug = title.ToLowerInvariant()
            .Replace("ã", "a").Replace("õ", "o").Replace("á", "a")
            .Replace("é", "e").Replace("í", "i").Replace("ó", "o")
            .Replace("ú", "u").Replace("â", "a").Replace("ê", "e")
            .Replace("ô", "o").Replace("ç", "c");
        slug = SlugRegex().Replace(slug, "");
        slug = SpacesRegex().Replace(slug, "-");
        slug = slug.Trim('-');
        return slug;
    }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex SlugRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex SpacesRegex();
}
