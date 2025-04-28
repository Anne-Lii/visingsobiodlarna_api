using System.Text.Json.Serialization;

namespace visingsobiodlarna_backend.DTOs;
public class PendingUserDto
{
    [JsonPropertyName("fullName")]
    public string? FullName { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }
}