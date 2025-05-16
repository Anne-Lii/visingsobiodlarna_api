using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.Models;
using visingsobiodlarna_backend.Services;
using Microsoft.EntityFrameworkCore;

namespace visingsobiodlarna_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IBlobService _blobService;

    public DocumentsController(ApplicationDbContext context, IBlobService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDocuments()
    {
        var documents = await _context.Documents
            .OrderByDescending(d => d.UploadDate)
            .ToListAsync();
        return Ok(documents);
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UploadDocument([FromForm] IFormFile file, [FromForm] string title, [FromForm] string category)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Ingen fil vald.");

        var fileUrl = await _blobService.UploadFileAsync(file);

        var document = new DocumentModel
        {
            Title = title,
            Category = category,
            FileUrl = fileUrl,
            UploadDate = DateTime.UtcNow
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return Ok(document);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> DeleteDocument(int id)
    {
        var document = await _context.Documents.FindAsync(id);
        if (document == null)
            return NotFound();

        var deleted = await _blobService.DeleteFileAsync(document.FileUrl);
        if (!deleted)
            return StatusCode(500, "Kunde inte radera filen från blob storage.");

        _context.Documents.Remove(document);
        await _context.SaveChangesAsync();

        return NoContent();
    }


    [HttpGet("{id}/downloadlink")]
    public async Task<IActionResult> GetDownloadLink(int id)
    {
        var document = await _context.Documents.FindAsync(id);
        if (document == null)
            return NotFound();

        var blobName = new Uri(document.FileUrl).Segments.Last(); //Extraherar blob-namn från URL
        var sasUrl = _blobService.GetSasUriForBlob(blobName);
        return Ok(new { url = sasUrl });
    }

}
