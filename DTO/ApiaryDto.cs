using System.Text.Json.Serialization;

namespace visingsobiodlarna_backend.DTOs;

public class ApiaryDto
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("location")]
    public string Location { get; set; } = null!;

    [JsonPropertyName("hiveCount")]
    public int HiveCount { get; set; }
}