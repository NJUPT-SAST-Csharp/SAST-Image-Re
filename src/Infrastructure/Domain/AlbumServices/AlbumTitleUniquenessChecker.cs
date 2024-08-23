﻿using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.Exceptions;
using Domain.AlbumDomain.Services;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Domain.AlbumServices
{
    internal sealed class AlbumTitleUniquenessChecker(DomainDbContext context)
        : IAlbumTitleUniquenessChecker
    {
        private readonly DomainDbContext _context = context;

        public async Task CheckAsync(
            AlbumTitle title,
            CancellationToken cancellationToken = default
        )
        {
            bool isDuplicated = await _context
                .Albums.AsNoTracking()
                .AnyAsync(
                    album => EF.Property<AlbumTitle>(album, "_title") == title,
                    cancellationToken
                );

            if (isDuplicated)
            {
                AlbumTitleDuplicateException.Throw(title);
            }
        }
    }
}
