namespace HeadphoneStore.StoreFrontEnd.Common.Options;

public class ApiOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string ApiVersion { get; set; } = "v1";
    public int Timeout { get; set; } = 30;
    public int ExpiryMinutes { get; set; } = 30;
}
