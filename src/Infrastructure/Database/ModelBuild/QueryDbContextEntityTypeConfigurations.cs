using Application.AlbumServices;
using Application.CategoryServices;
using Application.ImageServices;
using Application.TagServices;
using Application.UserServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.ModelBuild
{
    internal sealed class QueryDbContextEntityTypeConfigurations
        : IEntityTypeConfiguration<AlbumModel>,
            IEntityTypeConfiguration<ImageModel>,
            IEntityTypeConfiguration<UserModel>,
            IEntityTypeConfiguration<CategoryModel>,
            IEntityTypeConfiguration<TagModel>
    {
        public void Configure(EntityTypeBuilder<AlbumModel> builder)
        {
            builder.HasQueryFilter(a => a.RemovedAt == null);

            builder.PrimitiveCollection(album => album.Collaborators);
            builder.HasOne<CategoryModel>().WithMany().HasForeignKey(album => album.CategoryId);
            builder.HasOne<UserModel>().WithMany().HasForeignKey(album => album.AuthorId);
            builder.HasMany(album => album.Images).WithOne().HasForeignKey(image => image.AlbumId);
            builder
                .HasMany<UserModel>()
                .WithMany()
                .UsingEntity<SubscribeModel>(
                    l =>
                        l.HasOne<UserModel>()
                            .WithMany(u => u.Subscribes)
                            .HasForeignKey(s => s.User),
                    r =>
                        r.HasOne<AlbumModel>()
                            .WithMany(a => a.Subscribes)
                            .HasForeignKey(s => s.Album),
                    s => s.ToTable("subscribes")
                );

            builder.HasIndex(album => album.Title).IsUnique(true);
        }

        public void Configure(EntityTypeBuilder<ImageModel> builder)
        {
            builder.HasQueryFilter(image => image.RemovedAt == null);

            builder.PrimitiveCollection(image => image.Tags);
            builder.HasOne<UserModel>().WithMany().HasForeignKey(image => image.AuthorId);
            builder.HasOne<AlbumModel>().WithMany().HasForeignKey(image => image.AlbumId);
            builder
                .HasMany<UserModel>()
                .WithMany()
                .UsingEntity<LikeModel>(
                    l => l.HasOne<UserModel>().WithMany(u => u.Likes).HasForeignKey(l => l.User),
                    r => r.HasOne<ImageModel>().WithMany(i => i.Likes).HasForeignKey(l => l.Image),
                    like => like.ToTable("likes")
                );
        }

        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasIndex(user => user.Username).IsUnique(true);
        }

        public void Configure(EntityTypeBuilder<CategoryModel> builder)
        {
            builder.HasIndex(c => c.Name).IsUnique(true);
        }

        public void Configure(EntityTypeBuilder<TagModel> builder)
        {
            builder.HasIndex(tag => tag.Name).IsUnique(true);
        }
    }
}
