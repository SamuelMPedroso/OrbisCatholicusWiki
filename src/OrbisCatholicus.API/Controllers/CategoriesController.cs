using Microsoft.AspNetCore.Mvc;
using OrbisCatholicus.Application.Interfaces;

namespace OrbisCatholicus.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _categoryService.GetAllAsync();
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var detail = await _categoryService.GetBySlugAsync(slug);
        if (!detail.Success)
            return detail.Error == "Categoria não encontrada." ? NotFound(detail.Error) : BadRequest(detail.Error);

        var articles = await _categoryService.GetArticlesByCategoryAsync(slug, page, pageSize);
        return Ok(new { Category = detail.Data, Articles = articles.Data });
    }
}
