namespace visingsobiodlarna_backend.Services;

public interface IBlobService
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<bool> DeleteFileAsync(string fileUrl);
}
