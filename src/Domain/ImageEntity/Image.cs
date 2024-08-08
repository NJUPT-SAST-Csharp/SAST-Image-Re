namespace Domain.ImageEntity
{
    public sealed class Image
    {
        private Image(ImageId id)
        {
            Id = id;
        }

        internal ImageId Id { get; }
    }
}
