using System.Text.Json.Serialization;

namespace ContactosApi.Models;

public class ErrorResponse
{
    [JsonPropertyName("mensaje")]
    public string Mensaje { get; init; } = string.Empty;
}
