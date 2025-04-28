using System.Text.Json.Serialization;

namespace visingsobiodlarna_backend.DTOs;

public class LoginDto
{
    [JsonPropertyName("email")] //Matchar frontendens camelCase
    public string? Email { get; set; }

    [JsonPropertyName("password")]
    public string? Password { get; set; }
}
