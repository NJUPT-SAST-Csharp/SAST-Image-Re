using System.ComponentModel.DataAnnotations;
using Application.Query;
using Application.TagServices.Queries;
using Domain.Command;
using Domain.TagDomain.Commands;
using Domain.TagDomain.TagEntity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;

namespace WebAPI.Controllers;

[Route("api/tags")]
[ApiController]
public sealed class TagController(
    IDomainCommandSender commandSender,
    IQueryRequestSender querySender
) : ControllerBase
{
    private readonly IDomainCommandSender _commandSender = commandSender;
    private readonly IQueryRequestSender _querySender = querySender;

    public sealed record CreateTagRequest(
        [Length(TagName.MinLength, TagName.MaxLength)] string Name
    );

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTag(
        [FromBody] CreateTagRequest request,
        CancellationToken cancellationToken
    )
    {
        if (TagName.TryCreateNew(request.Name, out var name) == false)
            return this.ValidationFail(name, nameof(request.Name));

        CreateTagCommand command = new(name, new(User));

        var id = await _commandSender.SendAsync(command, cancellationToken);

        return Ok(new { id });
    }

    [HttpGet]
    public async Task<IActionResult> GetTags(
        [FromQuery] [MaxLength(TagName.MaxLength)] string? name = null,
        CancellationToken cancellationToken = default
    )
    {
        TagsQuery query = new(name);

        var tags = await _querySender.SendAsync(query, cancellationToken);

        return Ok(tags);
    }

    public sealed record UpdateTagRequest(
        [Length(TagName.MinLength, TagName.MaxLength)] string Name
    );

    [Authorize(AuthPolicies.Admin)]
    [HttpPost("{id:long}")]
    public async Task<IActionResult> UpdateTag(
        [FromRoute] long id,
        [FromBody] UpdateTagRequest request,
        CancellationToken cancellationToken
    )
    {
        if (TagName.TryCreateNew(request.Name, out var name) == false)
            return this.ValidationFail(name, nameof(request.Name));

        UpdateTagCommand command = new(new(id), name, new(User));

        await _commandSender.SendAsync(command, cancellationToken);

        return NoContent();
    }

    [Authorize(AuthPolicies.Admin)]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteTag(
        [FromRoute] long id,
        CancellationToken cancellationToken
    )
    {
        DeleteTagCommand command = new(new(id), new(User));

        await _commandSender.SendAsync(command, cancellationToken);

        return NoContent();
    }
}
