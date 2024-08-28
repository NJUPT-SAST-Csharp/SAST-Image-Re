using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Utilities
{
    public static class ControllerExtensions
    {
        public static BadRequestObjectResult ValidationFail(
            this ControllerBase controller,
            object value,
            string? name = null
        )
        {
            var result = new ProblemDetails()
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
            return image is null ? controller.NotFound() : controller.File(image, "image/*");
        }
    }
}
