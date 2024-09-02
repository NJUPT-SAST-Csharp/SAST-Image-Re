namespace Infrastructure.SharedServices.Storage
{
    internal sealed class StorageOptions
    {
        public required string ImagePath { get; init; }
        public required string CoverPath { get; init; }
    }
}
