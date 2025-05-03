using System.Text.Json.Serialization;

namespace HeadphoneStore.StoreFrontEnd.Models;

public class ErrorResponse
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string TraceId { get; set; } = string.Empty;
    [JsonPropertyName("errorCodes")]
    public string[]? ErrorCodes { get; set; }
    [JsonPropertyName("errors")]
    public Dictionary<string, string[]>? Errors { get; set; }
}