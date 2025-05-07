using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace visingsobiodlarna_backend.DTOs;

public class MiteDto
{
    [Required]
    [JsonPropertyName("hiveId")]
    public int HiveId { get; set; }

    [Required]
    [Range(1900, 3000, ErrorMessage = "Ogiltigt år XXXX")]
    [JsonPropertyName("year")]
    public int Year { get; set; }

    [Required]
    [Range(1, 53, ErrorMessage = "Vecka måste vara mellan 1 och 53")]
    [JsonPropertyName("week")]
    public int Week { get; set; }

    [Required]
    [Range(0, 10000, ErrorMessage = "Kvalsterantal måste vara mellan 0 och 1000")]
    [JsonPropertyName("miteCount")]
    public int MiteCount { get; set; }
}
