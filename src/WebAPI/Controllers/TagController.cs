using System.ComponentModel.DataAnnotations;
using Application.Query;
using Application.TagServices.Queries;
using Domain.Command;
using Domain.TagDomain.Commands;
using Domain.TagDomain.TagEntity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;

namespace WebAPI.Controllers
{
    [Route("api")]
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

        [HttpPost("tag")]
        public async Task<IActionResult> CreateTag(
            [FromBody] CreateTagRequest request,
            CancellationToken cancellationToken
        )
        {
            if (TagName.TryCreateNew(request.Name, out var name) == false)
                return this.ValidationFail(name, nameof(request.Name));

            CreateTagCommand command = new(name, new(User));

            var id = await _commandSender.SendAsync(command, cancellationToken);

            return Ok(id);
        }

        [HttpGet("tags")]
        public async Task<IActionResult> GetTags(
            [FromQuery] [MaxLength(TagName.MaxLength)] string? name = null,
            CancellationToken cancellationToken = default
        )
        {
            TagsQuery query = new(name);

            var tags = await _querySender.SendAsync(query, cancellationToken);

            return Ok(tags);
        }
    }
}
