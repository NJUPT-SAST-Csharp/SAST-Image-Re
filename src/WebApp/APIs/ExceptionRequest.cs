using System.Net;

namespace WebApp.APIs;

public readonly record struct ExceptionRequest(HttpStatusCode StatusCode, string Message) { };
