using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace visingsobiodlarna_backend.DTOs
{
    public class HiveDto
    {
        [JsonPropertyName("id")]// Matchar frontendens camelCase
        public int Id { get; set; }

        [Required(ErrorMessage = "Namn är obligatoriskt")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [Required]
        [JsonPropertyName("apiaryId")]
        public int ApiaryId { get; set; }
    }
}
