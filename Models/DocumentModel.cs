namespace visingsobiodlarna_backend.Models;

public class DocumentModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public string Category { get; set; } = null!;
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
}
