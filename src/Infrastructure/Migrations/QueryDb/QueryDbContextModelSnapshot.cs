﻿// <auto-generated />
using System;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.QueryDb
{
    [DbContext(typeof(QueryDbContext))]
    partial class QueryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("query")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Application.AlbumServices.AlbumModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("AccessLevel")
                        .HasColumnType("integer")
                        .HasColumnName("access_level");

                    b.Property<long>("AuthorId")
                        .HasColumnType("bigint")
                        .HasColumnName("author_id");

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint")
                        .HasColumnName("category_id");

                    b.Property<long[]>("Collaborators")
                        .IsRequired()
                        .HasColumnType("bigint[]")
                        .HasColumnName("collaborators");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<DateTime?>("RemovedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("removed_at");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_albums");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_albums_author_id");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_albums_category_id");

                    b.HasIndex("Title")
                        .IsUnique()
                        .HasDatabaseName("ix_albums_title");

                    b.ToTable("albums", "query");
                });

            modelBuilder.Entity("Application.AlbumServices.SubscribeModel", b =>
                {
                    b.Property<long>("Album")
                        .HasColumnType("bigint")
                        .HasColumnName("album");

                    b.Property<long>("User")
                        .HasColumnType("bigint")
                        .HasColumnName("user");

                    b.HasKey("Album", "User")
                        .HasName("pk_subscribes");

                    b.HasIndex("User")
                        .HasDatabaseName("ix_subscribes_user");

                    b.ToTable("subscribes", "query");
                });

            modelBuilder.Entity("Application.CategoryServices.CategoryModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_categories");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_categories_name");

                    b.ToTable("categories", "query");
                });

            modelBuilder.Entity("Application.ImageServices.ImageModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<int>("AccessLevel")
                        .HasColumnType("integer")
                        .HasColumnName("access_level");

                    b.Property<long>("AlbumId")
                        .HasColumnType("bigint")
                        .HasColumnName("album_id");

                    b.Property<long>("AuthorId")
                        .HasColumnType("bigint")
                        .HasColumnName("author_id");

                    b.Property<long[]>("Collaborators")
                        .IsRequired()
                        .HasColumnType("bigint[]")
                        .HasColumnName("collaborators");

                    b.Property<DateTime?>("RemovedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("removed_at");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<long[]>("Tags")
                        .IsRequired()
                        .HasColumnType("bigint[]")
                        .HasColumnName("tags");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("uploaded_at");

                    b.Property<long>("UploaderId")
                        .HasColumnType("bigint")
                        .HasColumnName("uploader_id");

                    b.HasKey("Id")
                        .HasName("pk_images");

                    b.HasIndex("AlbumId")
                        .HasDatabaseName("ix_images_album_id");

                    b.HasIndex("AuthorId")
                        .HasDatabaseName("ix_images_author_id");

                    b.HasIndex("UploaderId")
                        .HasDatabaseName("ix_images_uploader_id");

                    b.ToTable("images", "query");
                });

            modelBuilder.Entity("Application.ImageServices.LikeModel", b =>
                {
                    b.Property<long>("Image")
                        .HasColumnType("bigint")
                        .HasColumnName("image");

                    b.Property<long>("User")
                        .HasColumnType("bigint")
                        .HasColumnName("user");

                    b.HasKey("Image", "User")
                        .HasName("pk_likes");

                    b.HasIndex("User")
                        .HasDatabaseName("ix_likes_user");

                    b.ToTable("likes", "query");
                });

            modelBuilder.Entity("Application.TagServices.TagModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_tags");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_tags_name");

                    b.ToTable("tags", "query");
                });

            modelBuilder.Entity("Application.UserServices.UserModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text")
                        .HasColumnName("avatar_url");

                    b.Property<string>("Biography")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("biography");

                    b.Property<string>("HeaderUrl")
                        .HasColumnType("text")
                        .HasColumnName("header_url");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("ix_users_username");

                    b.ToTable("users", "query");
                });

            modelBuilder.Entity("Application.AlbumServices.AlbumModel", b =>
                {
                    b.HasOne("Application.UserServices.UserModel", null)
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_albums_users_author_id");

                    b.HasOne("Application.CategoryServices.CategoryModel", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_albums_categories_category_id");
                });

            modelBuilder.Entity("Application.AlbumServices.SubscribeModel", b =>
                {
                    b.HasOne("Application.AlbumServices.AlbumModel", null)
                        .WithMany("Subscribes")
                        .HasForeignKey("Album")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_subscribes_albums_album");

                    b.HasOne("Application.UserServices.UserModel", null)
                        .WithMany()
                        .HasForeignKey("User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_subscribes_users_user");
                });

            modelBuilder.Entity("Application.ImageServices.ImageModel", b =>
                {
                    b.HasOne("Application.AlbumServices.AlbumModel", null)
                        .WithMany("Images")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_images_albums_album_id");

                    b.HasOne("Application.UserServices.UserModel", null)
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_images_users_author_id");

                    b.HasOne("Application.UserServices.UserModel", null)
                        .WithMany()
                        .HasForeignKey("UploaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_images_users_uploader_id");
                });

            modelBuilder.Entity("Application.ImageServices.LikeModel", b =>
                {
                    b.HasOne("Application.ImageServices.ImageModel", null)
                        .WithMany("Likes")
                        .HasForeignKey("Image")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_likes_images_image");

                    b.HasOne("Application.UserServices.UserModel", null)
                        .WithMany()
                        .HasForeignKey("User")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_likes_users_user");
                });

            modelBuilder.Entity("Application.AlbumServices.AlbumModel", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("Subscribes");
                });

            modelBuilder.Entity("Application.ImageServices.ImageModel", b =>
                {
                    b.Navigation("Likes");
                });
#pragma warning restore 612, 618
        }
    }
}
