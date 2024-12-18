using Domain.CategoryDomain.CategoryEntity;

namespace Domain.CategoryDomain.Exceptions;

public sealed class CategoryNameDuplicateException(CategoryName name) : Exception
{
    public CategoryName Name { get; } = name;
}
