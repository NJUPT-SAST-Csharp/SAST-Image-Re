using System.ComponentModel.DataAnnotations;
using Application.Query;
using Domain.CategoryDomain.CategoryEntity;
using Domain.CategoryDomain.Commands;
using Domain.Command;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Utilities;

namespace WebAPI.Controllers;

[Route("api/categories")]
[ApiController]
[ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
public sealed class CategoryController(
    IDomainCommandSender commandSender,
    IQueryRequestSender querySender
) : ControllerBase
{
    private readonly IDomainCommandSender _commanderSender = commandSender;
    private readonly IQueryRequestSender _querySender = querySender;

    #region [Command/Post]

    public sealed record class CreateCategoryRequest(
        [Length(CategoryName.MinLength, CategoryName.MaxLength)] string Name,
        [MaxLength(CategoryDescription.MaxLength)] string Description
    );

    [HttpPost]
    public async Task<IActionResult> Create(
        [Required] [FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken
    )
    {
        if (CategoryName.TryCreateNew(request.Name, out var name) == false)
            return this.ValidationFail(request.Name, nameof(request.Name));
        if (CategoryDescription.TryCreateNew(request.Description, out var description) == false)
            return this.ValidationFail(request.Description, nameof(request.Description));
        CreateCategoryCommand command = new(name, description, new(User));
        var id = await _commanderSender.SendAsync(command, cancellationToken);
        return Ok(new { id });
    }

    //[HttpPost("{id:long}/remove")]
    //public async Task<IActionResult> Remove(
    //    [FromRoute] long id,
    //    CancellationToken cancellationToken
    //)
    //{
    //    var command = new RemoveCategoryCommand(new(id), new(User));
    //    await _commanderSender.SendAsync(command, cancellationToken);
    //    return NoContent();
    //}

    #endregion

    #region [Query/Get]

    //[HttpGet]
    //public async Task<IActionResult> Get(CancellationToken cancellationToken)
    //{
    //    var query = new GetCategoryListQuery();
    //    var result = await _querySender.SendAsync(query, cancellationToken);
    //    return Ok(result);
    //}

    //[HttpGet("{id:long}")]
    //public async Task<IActionResult> Get([FromRoute] long id, CancellationToken cancellationToken)
    //{
    //    var query = new GetCategoryQuery(new(id));
    //    var result = await _querySender.SendAsync(query, cancellationToken);
    //    return Ok(result);
    //}
    #endregion
}
