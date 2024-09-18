using System.Diagnostics.CodeAnalysis;
using Domain.CategoryDomain.CategoryEntity;
using Domain.Extensions;

namespace Domain.AlbumDomain.Exceptions;

public sealed class CategoryNotFoundException(CategoryId category) : DomainException
{
    public CategoryId Category { get; } = category;

    [DoesNotReturn]
    public static void Throw(CategoryId category) => throw new CategoryNotFoundException(category);
}
