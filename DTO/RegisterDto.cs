using System.Text.Json.Serialization;

namespace visingsobiodlarna_backend.DTOs
{
    public class RegisterDto
    {
        [JsonPropertyName("firstName")]//Matchar frontendens camelCase-fält
        public string? FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}