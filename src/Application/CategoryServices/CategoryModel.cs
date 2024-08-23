namespace Application.CategoryServices
{
    public sealed class CategoryModel
    {
        public required long Id { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
    }
}
