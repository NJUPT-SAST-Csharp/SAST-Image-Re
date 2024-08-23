using Domain.Entity;

namespace Domain.CategoryDomain.CategoryEntity
{
    public sealed class Category : EntityBase<CategoryId>
    {
        private Category()
            : base(default) { }

        private readonly CategoryName _name;
    }
}
