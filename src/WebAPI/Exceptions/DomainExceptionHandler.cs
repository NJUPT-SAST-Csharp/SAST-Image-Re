using Domain.AlbumDomain.Exceptions;
using Domain.Extensions;
using Domain.Shared;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Exceptions
{
    public sealed class DomainExceptionHandler : IExceptionHandler
    {
        public ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            if (exception is not DomainException)
                return ValueTask.FromResult(false);

            ProblemDetails response = exception switch
            {
                AlbumImmutableException => new()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Album's been removed or archived.",
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
                },

                AlbumTitleDuplicateException ex => new()
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = $"The title [{ex.Title.Value}] has been occupied.",
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10",
                },

                CategoryNotFoundException ex => new()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = $"Couldn't find the category with id [{ex.Category.Value}].",
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
                },

                ImageNotFoundException ex => new()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = $"Couldn't find image with id [{ex.ImageId.Value}] in this album.",
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
                },

                ImageTagsNotFoundException => new()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = $"Couldn't find tags.",
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
                },

                NoPermissionException => new()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = $"Couldn't find entity.",
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
                },

                EntityNotFoundException => new()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = $"Couldn't find entity.",
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
                },

                FileNotImageException => new()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Uploaded file type is not image.",
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
                },

                _ => new()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Unknown domain exception.",
                    Detail = exception.Message,
                    Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
                },
            };

            httpContext.Response.StatusCode = response.Status!.Value;
            httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return ValueTask.FromResult(true);
        }
    }
}
