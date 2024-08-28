﻿using Application.AlbumServices;
using Domain.AlbumDomain.AlbumEntity;
using Domain.Extensions;
using Domain.Shared;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Application.AlbumServices
{
    internal sealed class AlbumModelRepository(QueryDbContext context)
        : IRepository<AlbumModel, AlbumId>
    {
        private readonly QueryDbContext _context = context;

        public async Task<AlbumModel> GetAsync(
            AlbumId id,
            CancellationToken cancellationToken = default
        )
        {
            var album = await _context
                .Albums.IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id.Value, cancellationToken);

            if (album is null)
                EntityNotFoundException.Throw(id);

            return album;
        }

        public Task<AlbumModel?> GetOrDefaultAsync(
            AlbumId id,
            CancellationToken cancellationToken = default
        )
        {
            return _context
                .Albums.IgnoreQueryFilters()
                .FirstOrDefaultAsync(a => a.Id == id.Value, cancellationToken);
        }

        public async Task<AlbumId> AddAsync(
            AlbumModel entity,
            CancellationToken cancellationToken = default
        )
        {
            var album = await _context.Albums.AddAsync(entity, cancellationToken);
            return new(album.Entity.Id);
        }
    }
}