using Microsoft.AspNetCore.Mvc;
using OrbisCatholicus.Application.Interfaces;

namespace OrbisCatholicus.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IArticleService _articleService;

    public SearchController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest("O termo de busca é obrigatório.");

        var result = await _articleService.SearchAsync(q.Trim(), page, pageSize);
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }
}
