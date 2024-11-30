using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Utilities;

public static class ControllerExtensions
{
    public static BadRequestObjectResult ValidationFail<T>(
        this ControllerBase controller,
        T? value,
        string? name = null
    )
    {
        ProblemDetails result =
            new()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = name is null
                    ? $"The value [{value}] is invalid."
                    : $"The value of [{name}]: [{value}] is invalid.",
                Title = "Validation failed.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1",
            };

        return controller.BadRequest(result);
    }

    public static IActionResult DataOrNotFound(this ControllerBase controller, object? data)
    {
        return data is null ? controller.NotFound() : controller.Ok(data);
    }

    public static IActionResult ImageOrNotFound(this ControllerBase controller, Stream? image)
    {
        return image is null
            ? controller.PhysicalFile(
                $"{Environment.CurrentDirectory}/src/not_found.png",
                "image/png"
            )
            : controller.File(image, "image/*");
    }

    public static IActionResult AvatarOrNotFound(this ControllerBase controller, Stream? avatar)
    {
        return avatar is null
            ? controller.PhysicalFile(
                $"{Environment.CurrentDirectory}/src/account.png",
                "image/png"
            )
            : controller.File(avatar, "image/*");
    }

    public static IActionResult HeaderOrNotFound(this ControllerBase controller, Stream? header)
    {
        return header is null ? controller.NoContent() : controller.File(header, "image/*");
    }
}
