using Domain.AlbumEntity;
using Domain.CategoryEntity;
using Domain.TagEntity;
using Domain.UserEntity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    internal sealed class DatabaseContext : DbContext
    {
        public DbSet<Album> Albums { get; init; }
        public DbSet<Category> Categories { get; init; }
        public DbSet<Tag> Tags { get; init; }
        public DbSet<User> Users { get; init; }
    }
}
