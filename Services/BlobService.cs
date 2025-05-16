using Azure.Storage.Blobs;
using visingsobiodlarna_backend.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Sas;

namespace visingsobiodlarna_backend.Services;

public class BlobService : IBlobService
{
    private readonly AzureBlobStorageSettings _settings;

    public BlobService(IOptions<AzureBlobStorageSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + "_" + file.FileName);

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream);

        return blobClient.Uri.ToString();
    }

    public async Task<bool> DeleteFileAsync(string fileUrl)
    {
        var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);

        var blobName = new Uri(fileUrl).Segments.Last();
        var blobClient = containerClient.GetBlobClient(blobName);

        return await blobClient.DeleteIfExistsAsync();
    }



 public string GetSasUriForBlob(string blobName)
{
    var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
    var containerClient = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);
    var blobClient = containerClient.GetBlobClient(blobName);

    var sasBuilder = new BlobSasBuilder
    {
        BlobContainerName = _settings.ContainerName,
        BlobName = blobName,
        Resource = "b",
        ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
    };

    sasBuilder.SetPermissions(BlobSasPermissions.Read);

    var sasUri = blobClient.GenerateSasUri(sasBuilder);
    return sasUri.ToString();
}


}
