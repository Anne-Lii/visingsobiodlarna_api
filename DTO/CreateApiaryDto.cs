using System.Text.Json.Serialization;

namespace visingsobiodlarna_backend.DTOs;

public class CreateApiaryDto
{
    [JsonPropertyName("name")] //Matchar frontendens camelCase
    public string Name { get; set; } = null!;
    
    [JsonPropertyName("location")]
    public string Location { get; set; } = null!;
}