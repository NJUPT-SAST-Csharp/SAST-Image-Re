using Domain.AlbumDomain.AlbumEntity;
using Domain.CategoryDomain.CategoryEntity;
using Domain.TagDomain.TagEntity;
using Domain.UserDomain.UserEntity;
using Infrastructure.Database.ModelBuild;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    internal sealed class DomainDbContext(DbContextOptions<DomainDbContext> options)
        : DbContext(options)
    {
        public DbSet<Album> Albums { get; init; }
        public DbSet<Category> Categories { get; init; }
        public DbSet<Tag> Tags { get; init; }
        public DbSet<User> Users { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("domain");

            var configuration = new DomainDbContextEntityTypeConfigurations();
            modelBuilder.ApplyConfiguration<Album>(configuration);
            modelBuilder.ApplyConfiguration<Category>(configuration);
            modelBuilder.ApplyConfiguration<Tag>(configuration);
            modelBuilder.ApplyConfiguration<User>(configuration);
        }
    }
}
