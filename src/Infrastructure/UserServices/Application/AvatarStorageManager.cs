using Application.UserServices;
using Domain.UserDomain.UserEntity;
using Infrastructure.SharedServices.Storage;

namespace Infrastructure.UserServices.Application;

internal sealed class AvatarStorageManager(IStorageManager manager, ICompressProcessor processor)
    : IAvatarStorageManager
{
    public Stream? OpenReadStream(UserId user)
    {
        return manager.FindFile(user.Value.ToString(), StorageKind.Avatar);
    }

    public async Task UpdateAsync(UserId user, Stream avatar, CancellationToken cancellationToken)
    {
        string id = user.Value.ToString() + ".webp";

        var file = await processor.CompressAsync(avatar, 50, cancellationToken);

        await manager.StoreAsync(id, file, StorageKind.Avatar, cancellationToken);
    }
}
