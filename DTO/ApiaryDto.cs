using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace visingsobiodlarna_backend.DTOs;

public class ApiaryDto
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    [Required(ErrorMessage = "Namn är obligatoriskt")]
    [JsonPropertyName("name")]    
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Plats är obligatoriskt")]
    [JsonPropertyName("location")]
    public string Location { get; set; } = null!;

    [JsonPropertyName("hiveCount")]
    public int HiveCount { get; set; }
}