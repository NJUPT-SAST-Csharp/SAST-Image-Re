using System.ComponentModel.DataAnnotations;
using Application.Query;
using Application.UserServices.Queries;
using Domain.Command;
using Domain.UserDomain.Commands;
using Domain.UserDomain.UserEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;
using WebAPI.Utilities.Attributes;

namespace WebAPI.Controllers;

[Route("api/users")]
[ApiController]
public sealed class UserController(
    IDomainCommandSender commandSender,
    IQueryRequestSender querySender
) : ControllerBase
{
    private readonly IDomainCommandSender _commandSender = commandSender;
    private readonly IQueryRequestSender _querySender = querySender;

    public sealed record UpdateBiographyRequest([MaxLength(Biography.MaxLength)] string Biography);

    [Authorize]
    [HttpPost("biography")]
    public async Task<IActionResult> UpdateBiography(
        [FromBody] UpdateBiographyRequest request,
        CancellationToken cancellationToken
    )
    {
        if (Biography.TryCreateNew(request.Biography, out var bio) == false)
            return this.ValidationFail(request.Biography, nameof(request.Biography));

        UpdateBiographyCommand command = new(bio, new(User));

        await _commandSender.SendAsync(command, cancellationToken);

        return NoContent();
    }

    [Authorize]
    [HttpPost("avatar")]
    public async Task<IActionResult> UpdateAvatar(
        [FileValidator(0, 3)] [Required] IFormFile avatar,
        CancellationToken cancellationToken
    )
    {
        UpdateAvatarCommand command = new(avatar.OpenReadStream(), new(User));
        await _commandSender.SendAsync(command, cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpPost("header")]
    public async Task<IActionResult> UpdateHeader(
        [FileValidator(0, 10)] IFormFile header,
        CancellationToken cancellationToken
    )
    {
        UpdateHeaderCommand command = new(header.OpenReadStream(), new(User));
        await _commandSender.SendAsync(command, cancellationToken);
        return NoContent();
    }

    [HttpGet("avatar/{id:long}")]
    public async Task<IActionResult> GetAvatar(
        [FromRoute] long id,
        CancellationToken cancellationToken
    )
    {
        var query = new UserAvatarQuery(new(id));
        var result = await _querySender.SendAsync(query, cancellationToken);
        return this.AvatarOrNotFound(result);
    }

    [HttpGet("header/{id:long}")]
    public async Task<IActionResult> GetHeader(
        [FromRoute] long id,
        CancellationToken cancellationToken
    )
    {
        var query = new UserHeaderQuery(new(id));
        var result = await _querySender.SendAsync(query, cancellationToken);
        return this.HeaderOrNotFound(result);
    }

    [HttpGet("profile/{id:long}")]
    public async Task<IActionResult> GetProfileInfo(
        [FromRoute] long id,
        CancellationToken cancellationToken
    )
    {
        var query = new UserProfileQuery(new(id));
        var result = await _querySender.SendAsync(query, cancellationToken);
        return this.DataOrNotFound(result);
    }
}
