using System.ComponentModel.DataAnnotations;
using Application.Query;
using Domain.AlbumDomain.Commands;
using Domain.AlbumDomain.ImageEntity;
using Domain.Command;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities.Attributes;

namespace WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ImageController(
        IDomainCommandSender commandSender,
        IQueryRequestSender querySender
    ) : ControllerBase
    {
        private readonly IDomainCommandSender _commandSender = commandSender;
        private readonly IQueryRequestSender _querySender = querySender;

        public sealed record AddImageRequest(
            [MaxLength(ImageTitle.MaxLength)] string Title,
            [Length(0, 10)] long[] Tags,
            [FileValidator(0, 50)] IFormFile Image
        );

        [RequestSizeLimit(1024 * 1024 * 50)]
        [HttpPost("album/{albumId:long}/add")]
        public async Task<IActionResult> AddImage(
            [FromRoute] long albumId,
            [Required] [FromForm] AddImageRequest request,
            CancellationToken cancellationToken
        )
        {
            if (ImageTitle.TryCreateNew(request.Title, out var title) == false)
                return ValidationFail(request.Title, nameof(request.Title));
            if (ImageTags.TryCreateNew(request.Tags, out var tags) == false)
                return ValidationFail(request.Tags, nameof(request.Tags));

            AddImageCommand command =
                new(new(albumId), title, tags, request.Image.OpenReadStream(), new(User));

            await _commandSender.SendAsync(command, cancellationToken);

            return NoContent();
        }

        private BadRequestObjectResult ValidationFail(object value, string? name = null)
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

            return BadRequest(result);
        }
    }
}
