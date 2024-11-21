using System.Net;
using Refit;

namespace WebApp.APIs;

public readonly record struct ExceptionRequest(HttpStatusCode StatusCode, string Message)
{
    public ExceptionRequest(IApiResponse response)
        : this(response.StatusCode, "Network error.") { }

    public ExceptionRequest(string message)
        : this(HttpStatusCode.BadRequest, message) { }
};
