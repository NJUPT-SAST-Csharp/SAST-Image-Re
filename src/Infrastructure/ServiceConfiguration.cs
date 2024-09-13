using System.Data.Common;
using System.Text;
using Application;
using Application.AlbumServices;
using Application.AlbumServices.Queries;
using Application.ImageServices;
using Application.ImageServices.Queries;
using Application.Query;
using Application.SharedServices;
using Application.TagServices;
using Application.TagServices.Queries;
using Application.UserServices;
using Domain;
using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.AlbumDomain.Services;
using Domain.Command;
using Domain.Core.Event;
using Domain.Extensions;
using Domain.TagDomain.Services;
using Domain.TagDomain.TagEntity;
using Domain.UserDomain.Services;
using Domain.UserDomain.UserEntity;
using Domain.UserEntity.Services;
using Infrastructure.AlbumServices.Application;
using Infrastructure.AlbumServices.Domain;
using Infrastructure.Database;
using Infrastructure.ImageServices.Application;
using Infrastructure.SharedServices.EventBus;
using Infrastructure.SharedServices.Storage;
using Infrastructure.TagServices.Application;
using Infrastructure.TagServices.Domain;
using Infrastructure.UserServices.Application;
using Infrastructure.UserServices.Domain;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using StackExchange.Redis;

namespace Infrastructure
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddScoped<DbConnection>(_ => new NpgsqlConnection(
                    configuration.GetConnectionString("Database")
                ))
                .AddDbContext<DomainDbContext>(
                    (services, options) =>
                    {
                        options.UseNpgsql(services.GetRequiredService<DbConnection>());
                        options.UseSnakeCaseNamingConvention();
                    }
                )
                .AddDbContext<QueryDbContext>(
                    (services, options) =>
                    {
                        options.UseNpgsql(services.GetRequiredService<DbConnection>());
                        options.UseSnakeCaseNamingConvention();
                    }
                );

            services
                .AddMediatR(options =>
                {
                    options.NotificationPublisherType = typeof(ForeachAwaitPublisher);
                    options.RegisterServicesFromAssemblies(
                        DomainAssembly.Assembly,
                        ApplicationAssembly.Assembly
                    );
                    options.AutoRegisterRequestProcessors = true;
                    options.AddOpenRequestPostProcessor(typeof(UnitOfWorkPostProcessor<,>));
                })
                .AddScoped<IQueryRequestSender, QuerySender>()
                .AddScoped<IDomainCommandSender, CommandSender>()
                .AddScoped<IDomainEventPublisher, EventPublisher>()
                .AddScoped<IUnitOfWork, UnitOfWork>();

            services
                .Configure<StorageOptions>(configuration.GetRequiredSection("Storage"))
                .AddSingleton<IStorageManager, StorageManager>();

            services.AddSingleton<IConnectionMultiplexer>(_ =>
                ConnectionMultiplexer.Connect(configuration.GetConnectionString("Cache")!)
            );

            return services;
        }

        public static IServiceCollection AddAlbumServices(this IServiceCollection services)
        {
            services
                .AddScoped<IRepository<Album, AlbumId>, AlbumDomainRepository>()
                .AddScoped<IRepository<AlbumModel, AlbumId>, AlbumModelRepository>()
                .AddScoped<ICategoryExistenceChecker, CategoryExistenceChecker>()
                .AddScoped<ICollaboratorsExistenceChecker, CollaboratorsExistenceChecker>()
                .AddScoped<IAlbumTitleUniquenessChecker, AlbumTitleUniquenessChecker>()
                .AddScoped<ICoverStorageManager, CoverStorageManager>()
                .AddScoped<IAlbumAvailabilityChecker, AlbumAvailabilityChecker>();

            services
                .AddScoped<IQueryRepository<AlbumsQuery, List<AlbumDto>>, AlbumQueryRepository>()
                .AddScoped<
                    IQueryRepository<DetailedAlbumQuery, DetailedAlbum?>,
                    AlbumQueryRepository
                >()
                .AddScoped<
                    IQueryRepository<RemovedAlbumsQuery, List<RemovedAlbumDto>>,
                    AlbumQueryRepository
                >();
            return services;
        }

        public static IServiceCollection AddImageServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<ImageModel, ImageId>, ImageModelRepository>();
            services.AddScoped<IRepository<LikeModel, (ImageId, UserId)>, LikeModelRepository>();
            services.AddScoped<
                IRepository<SubscribeModel, (AlbumId, UserId)>,
                SubscribeModelRepository
            >();

            services
                .AddScoped<IQueryRepository<AlbumsQuery, List<AlbumDto>>, AlbumQueryRepository>()
                .AddScoped<IQueryRepository<TagsQuery, List<TagDto>>, TagQueryRepository>()
                .AddScoped<
                    IQueryRepository<DetailedAlbumQuery, DetailedAlbum?>,
                    AlbumQueryRepository
                >()
                .AddScoped<
                    IQueryRepository<RemovedAlbumsQuery, List<RemovedAlbumDto>>,
                    AlbumQueryRepository
                >()
                .AddScoped<
                    IQueryRepository<AlbumImagesQuery, List<AlbumImageDto>>,
                    ImageQueryRepository
                >()
                .AddScoped<
                    IQueryRepository<RemovedImagesQuery, List<RemovedImageDto>>,
                    ImageQueryRepository
                >()
                .AddScoped<
                    IQueryRepository<DetailedImageQuery, DetailedImage?>,
                    ImageQueryRepository
                >();

            services.AddScoped<IImageModelTagDeletedRepository, ImageModelTagDeletedRepository>();
            services.AddScoped<IImageAvailabilityChecker, ImageAvailabilityChecker>();
            services.AddScoped<IImageTagsExistenceChecker, ImageTagsExistenceChecker>();
            services.AddSingleton<IImageStorageManager, ImageStorageManager>();
            services.AddSingleton<ICompressProcessor, CompressProcessor>();

            return services;
        }

        public static IServiceCollection AddTagServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Tag, TagId>, TagDomainRepository>();
            services.AddScoped<IRepository<TagModel, TagId>, TagModelRepository>();
            services.AddScoped<ITagNameUniquenessChecker, TagNameUniquenessChecker>();

            services.AddScoped<IQueryRepository<TagsQuery, List<TagDto>>, TagQueryRepository>();

            return services;
        }

        public static IServiceCollection AddUserServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddScoped<IRepository<User, Username>, UserDomainRepository>()
                .AddScoped<IRepository<User, UserId>, UserDomainRepository>()
                .AddScoped<IUsernameUniquenessChecker, UsernameUniquenessChecker>()
                .AddScoped<IRegistryCodeChecker, RegistryCodeChecker>();

            services.AddScoped<IRepository<UserModel, UserId>, UserModelRepository>();

            services
                .Configure<JwtAuthOptions>(configuration.GetRequiredSection("Auth"))
                .AddSingleton<IPasswordGenerator, PasswordGenerator>()
                .AddSingleton<IPasswordValidator, PasswordValidator>()
                .AddSingleton<IJwtGenerator, JwtGenerator>();

            return services;
        }

        public static IServiceCollection AddJwtAuth(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var jwtOptions =
                configuration.GetRequiredSection("Auth").Get<JwtAuthOptions>()
                ?? throw new NullReferenceException();

            services
                .AddAuthentication(static options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var secKey = jwtOptions.SecKey;

                    options.TokenValidationParameters = new()
                    {
                        NameClaimType = "Username",
                        RoleClaimType = "Roles",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.Default.GetBytes(secKey)
                        ),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        LifetimeValidator = (notbefore, expires, _, _) =>
                        {
                            return DateTime.UtcNow > (notbefore ?? DateTime.MinValue)
                                && DateTime.UtcNow < (expires ?? DateTime.MaxValue);
                        },
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidAlgorithms = [jwtOptions.Algorithm],
                    };
                });

            return services;
        }
    }
}
