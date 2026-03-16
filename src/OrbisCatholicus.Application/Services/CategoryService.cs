using AutoMapper;
using OrbisCatholicus.Application.Common;
using OrbisCatholicus.Application.DTOs;
using OrbisCatholicus.Application.Interfaces;
using OrbisCatholicus.Domain.Interfaces;

namespace OrbisCatholicus.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public CategoryService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<CategoryListDto>>> GetAllAsync()
    {
        var categories = await _uow.Categories.GetWithArticleCountAsync();
        return Result<IEnumerable<CategoryListDto>>.Ok(
            _mapper.Map<IEnumerable<CategoryListDto>>(categories));
    }

    public async Task<Result<CategoryDetailDto>> GetBySlugAsync(string slug)
    {
        var category = await _uow.Categories.GetBySlugAsync(slug);
        if (category == null)
            return Result<CategoryDetailDto>.Fail("Categoria não encontrada.");

        return Result<CategoryDetailDto>.Ok(_mapper.Map<CategoryDetailDto>(category));
    }

    public async Task<Result<PagedResult<ArticleListDto>>> GetArticlesByCategoryAsync(string slug, int page = 1, int pageSize = 20)
    {
        var category = await _uow.Categories.GetBySlugAsync(slug);
        if (category == null)
            return Result<PagedResult<ArticleListDto>>.Fail("Categoria não encontrada.");

        var articles = await _uow.Articles.GetByCategorySlugAsync(slug, page, pageSize);
        var totalCount = await _uow.Articles.CountAsync(a => a.Category != null && a.Category.Slug == slug);

        var result = new PagedResult<ArticleListDto>
        {
            Items = _mapper.Map<IEnumerable<ArticleListDto>>(articles),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };

        return Result<PagedResult<ArticleListDto>>.Ok(result);
    }
}
