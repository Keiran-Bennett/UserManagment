using Microsoft.AspNetCore.Http;

namespace UserManagement.WebMS.Controllers;

public static class HttpRequestExtensions
{
    public static bool IsBlazorRequest(this HttpRequest httpRequest)
    {
        var acceptHeader = httpRequest.Headers["Accept"].ToString();
        return acceptHeader.Contains("application/json");
    }
}
