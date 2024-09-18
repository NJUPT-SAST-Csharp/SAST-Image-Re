using Application.AlbumServices;
using Application.CategoryServices;
using Application.ImageServices;
using Application.TagServices;
using Application.UserServices;
using Infrastructure.Database.ModelBuild;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

internal sealed class QueryDbContext(DbContextOptions<QueryDbContext> options) : DbContext(options)
{
    public DbSet<AlbumModel> Albums { get; init; }
    public DbSet<ImageModel> Images { get; init; }
    public DbSet<UserModel> Users { get; init; }
    public DbSet<CategoryModel> Categories { get; init; }
    public DbSet<TagModel> Tags { get; init; }
    public DbSet<LikeModel> Likes { get; init; }
    public DbSet<SubscribeModel> Subscribes { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("query");

        QueryDbContextEntityTypeConfigurations configuration = new();

        modelBuilder.ApplyConfiguration<AlbumModel>(configuration);
        modelBuilder.ApplyConfiguration<UserModel>(configuration);
        modelBuilder.ApplyConfiguration<CategoryModel>(configuration);
        modelBuilder.ApplyConfiguration<TagModel>(configuration);
        modelBuilder.ApplyConfiguration<ImageModel>(configuration);
        modelBuilder.ApplyConfiguration<LikeModel>(configuration);
        modelBuilder.ApplyConfiguration<SubscribeModel>(configuration);
    }
}
