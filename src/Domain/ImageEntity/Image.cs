using System.Diagnostics.CodeAnalysis;
using Domain.AlbumEntity.Commands;
using Domain.Entity;

namespace Domain.ImageEntity
{
    internal sealed class Image : EntityBase<ImageId>, IFactoryConstructor<Image, AddImageCommand>
    {
        private Image()
            : base(default) { }

        internal ImageTitle Title { get; private init; }

        public static bool TryCreateNew(
            AddImageCommand command,
            [NotNullWhen(true)] out Image? entity
        )
        {
            entity = new() { Title = command.Title };
            return true;
        }
    }
}
