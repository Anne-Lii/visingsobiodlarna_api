using System.Text.Json.Serialization;

namespace visingsobiodlarna_backend.DTOs
{
    public class HiveDto
    {
        [JsonPropertyName("id")]// Matchar frontendens camelCase
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("apiaryId")]
        public int ApiaryId { get; set; }
    }
}
