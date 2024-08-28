using System.ComponentModel.DataAnnotations;
using Application.AlbumServices.Queries;
using Application.Query;
using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Commands;
using Domain.Command;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities.Attributes;

namespace WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public sealed class AlbumController(
        IDomainCommandSender commandSender,
        IQueryRequestSender querySender
    ) : ControllerBase
    {
        private readonly IDomainCommandSender _commanderSender = commandSender;
        private readonly IQueryRequestSender _querySender = querySender;

        public sealed record class CreateAlbumRequest(
            [Length(AlbumTitle.MinLength, AlbumTitle.MaxLength)] string Title,
            [Length(AlbumDescription.MinLength, AlbumDescription.MaxLength)] string Description,
            [Range(0, long.MaxValue)] long CategoryId,
            [Range(Accessibility.MinValue, Accessibility.MaxValue)] int Accessibility
        );

        [HttpPost("album")]
        public async Task<IActionResult> Create(
            [Required] [FromBody] CreateAlbumRequest request,
            CancellationToken cancellationToken
        )
        {
            if (AlbumTitle.TryCreateNew(request.Title, out var title) == false)
                return ValidationFail(request.Title, nameof(request.Title));
            if (AlbumDescription.TryCreateNew(request.Description, out var description) == false)
                return ValidationFail(request.Description, nameof(request.Description));
            if (Accessibility.TryCreateNew(request.Accessibility, out var accessibility) == false)
                return ValidationFail(request.Accessibility, nameof(request.Accessibility));

            CreateAlbumCommand command =
                new(title, description, accessibility, new(request.CategoryId), new());

            var albumId = await _commanderSender.SendAsync(command, cancellationToken);

            return Ok(albumId);
        }

        [HttpPost("album/{id:long}/remove")]
        public async Task<IActionResult> Remove(
            [FromRoute] long id,
            CancellationToken cancellationToken
        )
        {
            RemoveAlbumCommand command = new(new(id), new());
            await _commanderSender.SendAsync(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("album/{id:long}/restore")]
        public async Task<IActionResult> Restore(
            [FromRoute] long id,
            CancellationToken cancellationToken
        )
        {
            RestoreAlbumCommand command = new(new(id), new());
            await _commanderSender.SendAsync(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("album/{id:long}/archive")]
        public async Task<IActionResult> Archive(
            [FromRoute] long id,
            CancellationToken cancellationToken
        )
        {
            ArchiveCommand command = new(new(id), new());
            await _commanderSender.SendAsync(command, cancellationToken);

            return NoContent();
        }

        public sealed record class UpdateAccessibilityRequest(
            [Range(Accessibility.MinValue, Accessibility.MaxValue)] int Accessibility
        );

        [HttpPost("album/{id:long}/accessibility")]
        public async Task<IActionResult> UpdateAccessibility(
            [FromRoute] long id,
            [Required] [FromBody] UpdateAccessibilityRequest request,
            CancellationToken cancellationToken
        )
        {
            if (Accessibility.TryCreateNew(request.Accessibility, out var accessibility) == false)
                return ValidationFail(request.Accessibility, nameof(request.Accessibility));

            UpdateAccessibilityCommand command = new(new(id), accessibility, new());
            await _commanderSender.SendAsync(command, cancellationToken);

            return NoContent();
        }

        public sealed record class UpdateDescriptionRequest(
            [Length(AlbumDescription.MinLength, AlbumDescription.MaxLength)] string Description
        );

        [HttpPost("album/{id:long}/description")]
        public async Task<IActionResult> UpdateDescription(
            [FromRoute] long id,
            [Required] [FromBody] UpdateDescriptionRequest request,
            CancellationToken cancellationToken
        )
        {
            if (AlbumDescription.TryCreateNew(request.Description, out var description) == false)
                return ValidationFail(request.Description, nameof(request.Description));

            UpdateAlbumDescriptionCommand command = new(new(id), description, new());
            await _commanderSender.SendAsync(command, cancellationToken);

            return NoContent();
        }

        public sealed record class UpdateTitleRequest(
            [Length(AlbumTitle.MinLength, AlbumTitle.MaxLength)] string Title
        );

        [HttpPost("album/{id:long}/title")]
        public async Task<IActionResult> UpdateTitle(
            [FromRoute] long id,
            [FromBody] UpdateTitleRequest request,
            CancellationToken cancellationToken
        )
        {
            if (AlbumTitle.TryCreateNew(request.Title, out var title) == false)
                return ValidationFail(request.Title, nameof(request.Title));

            UpdateAlbumTitleCommand command = new(new(id), title, new());
            await _commanderSender.SendAsync(command, cancellationToken);

            return NoContent();
        }

        public sealed record class UpdateCollaboratorsRequest(long[] Collaborators);

        [HttpPost("album/{id:long}/collaborators")]
        public async Task<IActionResult> UpdateCollaborators(
            [FromRoute] long id,
            [Required] [FromBody] UpdateCollaboratorsRequest request
        )
        {
            if (Collaborators.TryCreateNew(request.Collaborators, out var collaborators) == false)
                return ValidationFail(request.Collaborators, nameof(request.Collaborators));

            UpdateCollaboratorsCommand command = new(new(id), collaborators, new());
            await _commanderSender.SendAsync(command);

            return NoContent();
        }

        [HttpPost("album/{id:long}/cover")]
        [RequestFormLimits(MultipartBodyLengthLimit = 1024 * 1024 * 20)]
        public async Task<IActionResult> UpdateCover(
            [FromRoute] long id,
            [FromForm] [FileValidator(0, 5)] IFormFile? file = null,
            CancellationToken cancellationToken = default
        )
        {
            Stream? image = file?.OpenReadStream();
            UpdateCoverCommand command = new(new(id), image, new());
            await _commanderSender.SendAsync(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("album/{id:long}/subscribe")]
        public async Task<IActionResult> Subscribe(
            [FromRoute] long id,
            CancellationToken cancellationToken
        )
        {
            SubscribeCommand command = new(new(id), new());

            await _commanderSender.SendAsync(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("album/{id:long}/unsubscribe")]
        public async Task<IActionResult> Unsubscribe(
            [FromRoute] long id,
            CancellationToken cancellationToken
        )
        {
            UnsubscribeCommand command = new(new(id), new());

            await _commanderSender.SendAsync(command, cancellationToken);

            return NoContent();
        }

        [HttpGet("album/{id:long}")]
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
        public async Task<IActionResult> GetAlbums(
            [FromQuery(Name = "c")] long? category = null,
            [FromQuery(Name = "a")] long? author = null,
            [FromQuery(Name = "t")]
            [Length(AlbumTitle.MinLength, AlbumTitle.MaxLength)]
                string? title = null,
            CancellationToken cancellationToken = default
        )
        {
            var result = await _querySender.SendAsync(
                new AlbumsQuery(category, author, title, new()),
                cancellationToken
            );

            return DataOrNotFound(result);
        }

        [HttpGet("albums/removed")]
        public async Task<IActionResult> GetRemovedAlbums()
        {
            var result = await _querySender.SendAsync(new RemovedAlbumsQuery(new()));
            return DataOrNotFound(result);
        }

        [HttpGet("album/cover/{id:long}")]
        public async Task<IActionResult> GetCover(long id, CancellationToken cancellationToken)
        {
            var result = await _querySender.SendAsync(
                new AlbumCoverQuery(id, new()),
                cancellationToken
            );

            return ImageOrNotFound(result);
        }

        private IActionResult DataOrNotFound(object? data)
        {
            return data is null ? NotFound() : Ok(data);
        }

        private IActionResult ImageOrNotFound(Stream? image)
        {
            return image is null ? NotFound() : File(image, "image/*");
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
