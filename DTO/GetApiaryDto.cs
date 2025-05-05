using System.Text.Json.Serialization;

namespace visingsobiodlarna_backend.DTOs;

public class GetApiaryDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("location")]
    public string Location { get; set; } = null!;
}