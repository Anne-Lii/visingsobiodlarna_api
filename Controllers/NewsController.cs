using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.DTOs;
using visingsobiodlarna_backend.Models;

namespace visingsobiodlarna_backend.Controllers;
[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public NewsController(ApplicationDbContext context)
    {
        _context = context;
    }

    //Hämtar alla nyheter
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NewsDto>>> GetNews()
{
    var newsList = await _context.NewsModels
        .OrderByDescending(n => n.PublishDate)
        .Select(n => new NewsDto
        {
            Id = n.Id,
            Title = n.Title,
            Content = n.Content,
            PublishDate = n.PublishDate.ToString("o") // ISO 8601
        })
        .ToListAsync();

    return Ok(newsList);
}

    //Hämtar en specifik nyhet baserat på id
    [HttpGet("{id}")]
    public async Task<ActionResult<NewsModel>> GetNewsItem(int id)
    {
        var newsItem = await _context.NewsModels.FindAsync(id);

        if (newsItem == null)
            return NotFound();

        return newsItem;
    }

    //Skapar en nyhet
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<NewsModel>> CreateNews(NewsModel newsItem)
    {
        newsItem.PublishDate = DateTime.UtcNow;

        _context.NewsModels.Add(newsItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNewsItem), new { id = newsItem.Id }, newsItem);
    }

    //Uppdaterar en nyhet baserat på ID
    [HttpPut("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UpdateNews(int id, NewsModel newsItem)
    {
        if (id != newsItem.Id)
            return BadRequest();

        _context.Entry(newsItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.NewsModels.Any(e => e.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    //Raderar en nyhet baserat på ID
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteNews(int id)
    {
        var newsItem = await _context.NewsModels.FindAsync(id);

        if (newsItem == null)
            return NotFound();

        _context.NewsModels.Remove(newsItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
