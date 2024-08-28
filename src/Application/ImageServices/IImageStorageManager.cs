﻿using Domain.AlbumDomain.ImageEntity;

namespace Application.ImageServices
{
    public interface IImageStorageManager
    {
        public Task StoreImageAsync(
            ImageId imageId,
            Stream imageFile,
            CancellationToken cancellationToken = default
        );
    }
}
