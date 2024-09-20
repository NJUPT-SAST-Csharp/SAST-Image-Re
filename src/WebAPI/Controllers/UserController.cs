using System.ComponentModel.DataAnnotations;
using Application.Query;
using Domain.Command;
using Domain.UserDomain.Commands;
using Domain.UserDomain.UserEntity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;

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
}
