using System.ComponentModel.DataAnnotations;
using Application.ImageServices.Queries;
using Application.Query;
using Domain.AlbumDomain.Commands;
using Domain.AlbumDomain.ImageEntity;
using Domain.Command;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;
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
                return this.ValidationFail(request.Title, nameof(request.Title));
            if (ImageTags.TryCreateNew(request.Tags, out var tags) == false)
                return this.ValidationFail(request.Tags, nameof(request.Tags));

            AddImageCommand command =
                new(new(albumId), title, tags, request.Image.OpenReadStream(), new(User));

            await _commandSender.SendAsync(command, cancellationToken);

            return NoContent();
        }

        [HttpGet("album/{albumId:long}/images")]
        public async Task<IActionResult> GetAlbumImages([FromRoute] long albumId)
        {
            AlbumImagesQuery query = new(new(albumId), new(User));

            var images = await _querySender.SendAsync(query);

            return this.DataOrNotFound(images);
        }
    }
}
