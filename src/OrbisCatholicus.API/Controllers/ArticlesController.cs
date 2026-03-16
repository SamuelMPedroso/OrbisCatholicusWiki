using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrbisCatholicus.Application.DTOs;
using OrbisCatholicus.Application.Interfaces;

namespace OrbisCatholicus.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService _articleService;

    public ArticlesController(IArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet("featured")]
    public async Task<IActionResult> GetFeatured([FromQuery] int count = 5)
    {
        var result = await _articleService.GetFeaturedAsync(count);
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("recent")]
    public async Task<IActionResult> GetRecent([FromQuery] int count = 10)
    {
        var result = await _articleService.GetRecentAsync(count);
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopular([FromQuery] int count = 10)
    {
        var result = await _articleService.GetPopularAsync(count);
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        var result = await _articleService.GetBySlugAsync(slug);
        if (!result.Success)
            return result.Error == "Artigo não encontrado." ? NotFound(result.Error) : BadRequest(result.Error);
        return Ok(result.Data);
    }

    [Authorize(Roles = "Autor,Revisor,Administrador")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArticleDto dto)
    {
        var userId = GetUserId();
        var result = await _articleService.CreateAsync(dto, userId);
        if (!result.Success) return BadRequest(result.Error);
        return CreatedAtAction(nameof(GetBySlug), new { slug = result.Data!.Slug }, result.Data);
    }

    [Authorize(Roles = "Autor,Revisor,Administrador")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateArticleDto dto)
    {
        var userId = GetUserId();
        var result = await _articleService.UpdateAsync(id, dto, userId);
        return result.Success ? NoContent() : BadRequest(result.Error);
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var userId = GetUserId();
        var result = await _articleService.DeactivateAsync(id, userId);
        return result.Success ? NoContent() : BadRequest(result.Error);
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
