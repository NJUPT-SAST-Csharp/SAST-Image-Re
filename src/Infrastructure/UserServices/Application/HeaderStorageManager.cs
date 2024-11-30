using Application.UserServices;
using Domain.UserDomain.UserEntity;
using Infrastructure.SharedServices.Storage;

namespace Infrastructure.UserServices.Application;

internal sealed class HeaderStorageManager(IStorageManager manager, ICompressProcessor processor)
    : IHeaderStorageManager
{
    public Stream? OpenReadStream(UserId user)
    {
        return manager.FindFile(user.Value.ToString(), StorageKind.Header);
    }

    public async Task UpdateAsync(UserId user, Stream header, CancellationToken cancellationToken)
    {
        string id = user.Value.ToString() + ".webp";

        var file = await processor.CompressAsync(header, 70, cancellationToken);

        await manager.StoreAsync(id, file, StorageKind.Header, cancellationToken);
    }
}
