using System.ComponentModel.DataAnnotations;
using Application.AlbumServices.Queries;
using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Commands;
using Domain.Command;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AlbumController(
        IDomainCommandSender commandSender,
        IQueryRequestSender querySender
    ) : ControllerBase
    {
        private readonly IDomainCommandSender _commanderSender = commandSender;
        private readonly IQueryRequestSender _querySender = querySender;

        public readonly record struct CreateAlbumRequest(
            [Length(AlbumTitle.MinLength, AlbumTitle.MaxLength)] string Title,
            [Length(AlbumDescription.MinLength, AlbumDescription.MaxLength)] string Description,
            [Range(0, long.MaxValue)] long CategoryId,
            [Range(Accessibility.MinValue, Accessibility.MaxValue)] int Accessibility
        );

        [HttpPost("album")]
        public async Task<IActionResult> CreateAlbum(
            [FromBody] CreateAlbumRequest request,
            CancellationToken cancellationToken
        )
        {
            if (
                AlbumTitle.TryCreateNew(request.Title, out var title)
                && AlbumDescription.TryCreateNew(request.Description, out var description)
                && Accessibility.TryCreateNew(request.Accessibility, out var accessibility)
            )
            {
                CreateAlbumCommand command =
                    new(title, description, accessibility, new(request.CategoryId), new());

                var albumId = await _commanderSender.SendAsync(command, cancellationToken);

                return Ok(albumId);
            }

            return BadRequest(request);
        }

        [HttpGet("album/{id}")]
        public async Task<IActionResult> GetDetailedAlbum(
            [FromRoute] long id,
            CancellationToken cancellationToken
        )
        {
            var result = await _querySender.SendAsync(
                new DetailedAlbumQuery(id, new()),
                cancellationToken
            );

            return DataOrNotFound(result);
        }

        [HttpGet("albums")]
        [HttpGet("albums/{categoryId}")]
        public async Task<IActionResult> GetAlbums(
            long? categoryId = null,
            CancellationToken cancellationToken = default
        )
        {
            var result = await _querySender.SendAsync(
                new AlbumsQuery(categoryId, new()),
                cancellationToken
            );
            return DataOrNotFound(result);
        }

        private IActionResult DataOrNotFound(object? data)
        {
            return data is null ? NotFound() : Ok(data);
        }
    }
}
