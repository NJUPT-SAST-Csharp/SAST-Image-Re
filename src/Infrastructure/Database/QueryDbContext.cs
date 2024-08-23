using Application.AlbumServices;
using Application.CategoryServices;
using Application.ImageServices;
using Application.UserServices;
using Infrastructure.Database.ModelBuild;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    internal sealed class QueryDbContext(DbContextOptions<QueryDbContext> options)
        : DbContext(options)
    {
        public DbSet<AlbumModel> Albums { get; init; }
        public DbSet<ImageModel> Images { get; init; }
        public DbSet<SubscribeModel> Subscribes { get; init; }
        public DbSet<UserModel> Users { get; init; }
        public DbSet<LikeModel> Likes { get; init; }
        public DbSet<CategoryModel> Categories { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("query");

            var configuration = new QueryDbContextEntityTypeConfigurations();

            modelBuilder.ApplyConfiguration<AlbumModel>(configuration);
            modelBuilder.ApplyConfiguration<ImageModel>(configuration);
        }
    }
}
