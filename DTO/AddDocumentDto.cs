using Microsoft.AspNetCore.Http;

namespace visingsobiodlarna_backend.DTOs;

public class AddDocumentDto
{
    public IFormFile File { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}
