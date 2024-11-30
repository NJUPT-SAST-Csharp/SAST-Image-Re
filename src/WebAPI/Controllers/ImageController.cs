using System.ComponentModel.DataAnnotations;
using Application.ImageServices;
using Application.ImageServices.Queries;
using Application.Query;
using Domain.AlbumDomain.Commands;
using Domain.AlbumDomain.ImageEntity;
using Domain.Command;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;
using WebAPI.Utilities.Attributes;

namespace WebAPI.Controllers;

[Route("api")]
[ApiController]
public class ImageController(IDomainCommandSender commandSender, IQueryRequestSender querySender)
    : ControllerBase
{
    private readonly IDomainCommandSender _commandSender = commandSender;
    private readonly IQueryRequestSender _querySender = querySender;

    #region [Command/Post]
    public sealed record AddImageRequest(
        [MaxLength(ImageTitle.MaxLength)] string Title,
        [FileValidator(0, 50)] IFormFile Image,
        [Length(0, 10)] long[]? Tags = null
    );

    [RequestSizeLimit(1024 * 1024 * 50)]
    [HttpPost("albums/{albumId:long}/add")]
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

    [HttpPost("albums/{albumId:long}/images/{imageId:long}/remove")]
    public async Task<IActionResult> Remove(
        [FromRoute] long albumId,
        [FromRoute] long imageId,
        CancellationToken cancellationToken
    )
    {
        RemoveImageCommand command = new(new(albumId), new(imageId), new(User));

        await _commandSender.SendAsync(command, cancellationToken);

        return NoContent();
    }

    [HttpPost("albums/{albumId:long}/images/{imageId:long}/restore")]
    public async Task<IActionResult> Restore(
        [FromRoute] long albumId,
        [FromRoute] long imageId,
        CancellationToken cancellationToken
    )
    {
        RestoreImageCommand command = new(new(albumId), new(imageId), new(User));

        await _commandSender.SendAsync(command, cancellationToken);

        return NoContent();
    }

    [HttpPost("albums/{albumId:long}/images/{imageId:long}/like")]
    public async Task<IActionResult> Like(
        [FromRoute] long albumId,
        [FromRoute] long imageId,
        CancellationToken cancellationToken
    )
    {
        LikeImageCommand command = new(new(albumId), new(imageId), new(User));

        await _commandSender.SendAsync(command, cancellationToken);

        return NoContent();
    }

    [HttpPost("albums/{albumId:long}/images/{imageId:long}/unlike")]
    public async Task<IActionResult> Unlike(
        [FromRoute] long albumId,
        [FromRoute] long imageId,
        CancellationToken cancellationToken
    )
    {
        UnlikeImageCommand command = new(new(albumId), new(imageId), new(User));

        await _commandSender.SendAsync(command, cancellationToken);

        return NoContent();
    }

    #endregion


    #region [Query/Get]

    [HttpGet("images/{id:long}/file")]
    public async Task<IActionResult> GetImage(
        [FromRoute] long id,
        [FromQuery] ImageKind kind = ImageKind.Thumbnail,
        CancellationToken cancellationToken = default
    )
    {
        ImageFileQuery query = new(new(id), kind, new(User));

        var image = await _querySender.SendAsync(query, cancellationToken);

        return this.ImageOrNotFound(image);
    }

    [Authorize]
    [HttpGet("images/{id:long}/info")]
    public async Task<IActionResult> GetDetailedImage(
        [FromRoute] long id,
        CancellationToken cancellationToken
    )
    {
        DetailedImageQuery query = new(new(id), new(User));

        var image = await _querySender.SendAsync(query, cancellationToken);

        return this.DataOrNotFound(image);
    }

    [HttpGet("albums/{albumId:long}/images")]
    public async Task<IActionResult> GetAlbumImages(
        [FromRoute] long albumId,
        CancellationToken cancellationToken
    )
    {
        AlbumImagesQuery query = new(new(albumId), new(User));

        var images = await _querySender.SendAsync(query, cancellationToken);

        return this.DataOrNotFound(images);
    }

    [Authorize]
    [HttpGet("albums/{albumId:long}/images/removed")]
    public async Task<IActionResult> GetRemovedImages(
        [FromRoute] long albumId,
        CancellationToken cancellationToken
    )
    {
        RemovedImagesQuery query = new(new(albumId), new(User));

        var images = await _querySender.SendAsync(query, cancellationToken);

        return this.DataOrNotFound(images);
    }

    #endregion
}
