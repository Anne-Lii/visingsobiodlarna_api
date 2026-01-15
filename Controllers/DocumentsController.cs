using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.Models;
using visingsobiodlarna_backend.Services;
using visingsobiodlarna_backend.DTOs;

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
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadDocument([FromForm] AddDocumentDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("Ingen fil vald.");

        var fileUrl = await _blobService.UploadFileAsync(dto.File);

        var document = new DocumentModel
        {
            Title = dto.Title,
            Category = dto.Category,
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

        var uri = new Uri(document.FileUrl);
        var blobName = WebUtility.UrlDecode(uri.Segments.Last());

        var sasUrl = _blobService.GetSasUriForBlob(blobName, document.Title);
        return Ok(new { url = sasUrl });
    }


}
