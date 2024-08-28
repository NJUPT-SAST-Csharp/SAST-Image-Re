﻿// <auto-generated />
using System;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(DomainDbContext))]
    [Migration("20240820044256_Domain_2")]
    partial class Domain_2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("domain")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.AlbumServices.AlbumEntity.Album", b =>
                {
                    b.Property<long>("Album")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<long>("_author")
                        .HasColumnType("bigint")
                        .HasColumnName("author_id");

                    b.Property<long[]>("_collaborators")
                        .IsRequired()
                        .HasColumnType("bigint[]")
                        .HasColumnName("collaborators");

                    b.Property<bool>("_isArchived")
                        .HasColumnType("boolean")
                        .HasColumnName("is_archived");

                    b.Property<bool>("_isRemoved")
                        .HasColumnType("boolean")
                        .HasColumnName("is_removed");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Album")
                        .HasName("pk_albums");

                    b.HasIndex("_title")
                        .IsUnique()
                        .HasDatabaseName("ix_albums_title");

                    b.ToTable("albums", "domain");
                });

            modelBuilder.Entity("Domain.CategoryDomain.CategoryEntity.Category", b =>
                {
                    b.Property<long>("Album")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("_name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Album")
                        .HasName("pk_categories");

                    b.ToTable("categories", "domain");
                });

            modelBuilder.Entity("Domain.TagDomain.TagEntity.Tag", b =>
                {
                    b.Property<long>("Album")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("_name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Album")
                        .HasName("pk_tags");

                    b.ToTable("tags", "domain");
                });

            modelBuilder.Entity("Domain.UserDomain.UserEntity.User", b =>
                {
                    b.Property<long>("Album")
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<int[]>("_roles")
                        .IsRequired()
                        .HasColumnType("integer[]")
                        .HasColumnName("roles");

                    b.Property<string>("_username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Album")
                        .HasName("pk_users");

                    b.ToTable("users", "domain");
                });

            modelBuilder.Entity("Domain.AlbumServices.AlbumEntity.Album", b =>
                {
                    b.OwnsMany("Domain.AlbumServices.AlbumEntity.Subscribe", "_subscribers", b1 =>
                        {
                            b1.Property<long>("User")
                                .HasColumnType("bigint")
                                .HasColumnName("user");

                            b1.Property<long>("Album")
                                .HasColumnType("bigint")
                                .HasColumnName("album");

                            b1.HasKey("User", "Album")
                                .HasName("pk_subscribers");

                            b1.HasIndex("Album")
                                .HasDatabaseName("ix_subscribers_album");

                            b1.ToTable("subscribers", "domain");

                            b1.WithOwner()
                                .HasForeignKey("Album")
                                .HasConstraintName("fk_subscribers_albums_album");

                            b1.HasOne("Domain.UserDomain.UserEntity.User", null)
                                .WithMany()
                                .HasForeignKey("User")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired()
                                .HasConstraintName("fk_subscribers_users_user");
                        });

                    b.OwnsOne("Domain.AlbumServices.AlbumEntity.Cover", "_cover", b1 =>
                        {
                            b1.Property<long>("Album")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<long?>("Album")
                                .HasColumnType("bigint")
                                .HasColumnName("cover_id");

                            b1.Property<bool>("IsLatestImage")
                                .HasColumnType("boolean")
                                .HasColumnName("cover_is_latest_image");

                            b1.HasKey("Album");

                            b1.ToTable("albums", "domain");

                            b1.WithOwner()
                                .HasForeignKey("Album")
                                .HasConstraintName("fk_albums_albums_id");
                        });

                    b.OwnsMany("Domain.AlbumServices.ImageEntity.Image", "_images", b1 =>
                        {
                            b1.Property<long>("Album")
                                .HasColumnType("bigint")
                                .HasColumnName("id");

                            b1.Property<long>("album_id")
                                .HasColumnType("bigint")
                                .HasColumnName("album_id");

                            b1.HasKey("Album")
                                .HasName("pk_images");

                            b1.HasIndex("album_id")
                                .HasDatabaseName("ix_images_album_id");

                            b1.ToTable("images", "domain");

                            b1.WithOwner()
                                .HasForeignKey("album_id")
                                .HasConstraintName("fk_images_albums_album_id");

                            b1.OwnsMany("Domain.AlbumServices.ImageEntity.LikeModel", "_likes", b2 =>
                                {
                                    b2.Property<long>("User")
                                        .HasColumnType("bigint")
                                        .HasColumnName("user");

                                    b2.Property<long>("Image")
                                        .HasColumnType("bigint")
                                        .HasColumnName("image");

                                    b2.HasKey("User", "Image")
                                        .HasName("pk_likes");

                                    b2.HasIndex("Image")
                                        .HasDatabaseName("ix_likes_image");

                                    b2.ToTable("likes", "domain");

                                    b2.WithOwner()
                                        .HasForeignKey("Image")
                                        .HasConstraintName("fk_likes_images_image");

                                    b2.HasOne("Domain.UserDomain.UserEntity.User", null)
                                        .WithMany()
                                        .HasForeignKey("User")
                                        .OnDelete(DeleteBehavior.Cascade)
                                        .IsRequired()
                                        .HasConstraintName("fk_likes_users_user");
                                });

                            b1.Navigation("_likes");
                        });

                    b.Navigation("_cover");

                    b.Navigation("_images");

                    b.Navigation("_subscribers");
                });
#pragma warning restore 612, 618
        }
    }
}