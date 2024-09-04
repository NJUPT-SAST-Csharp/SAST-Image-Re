using System.Data.Common;
using Application;
using Application.AlbumServices;
using Application.AlbumServices.Queries;
using Application.ImageServices;
using Application.ImageServices.Queries;
using Application.Query;
using Application.SharedServices;
using Application.TagServices;
using Application.TagServices.Queries;
using Domain;
using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.AlbumDomain.Services;
using Domain.Command;
using Domain.Core.Event;
using Domain.Extensions;
using Domain.TagDomain.Services;
using Domain.TagDomain.TagEntity;
using Domain.UserDomain.UserEntity;
using Infrastructure.AlbumServices.Application;
using Infrastructure.AlbumServices.Domain;
using Infrastructure.Database;
using Infrastructure.ImageServices.Application;
using Infrastructure.SharedServices.EventBus;
using Infrastructure.SharedServices.Storage;
using Infrastructure.TagServices.Application;
using Infrastructure.TagServices.Domain;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

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

            return services;
        }

        public static IServiceCollection AddAlbumServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Album, AlbumId>, AlbumDomainRepository>();
            services.AddScoped<ICategoryExistenceChecker, CategoryExistenceChecker>();
            services.AddScoped<ICollaboratorsExistenceChecker, CollaboratorsExistenceChecker>();
            services.AddScoped<IAlbumTitleUniquenessChecker, AlbumTitleUniquenessChecker>();

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

            services.AddScoped<
                IQueryRepository<DetailedAlbumQuery, DetailedAlbum?>,
                AlbumQueryRepository
            >();
            services.AddScoped<
                IQueryRepository<AlbumsQuery, List<AlbumDto>>,
                AlbumQueryRepository
            >();
            services.AddScoped<
                IQueryRepository<RemovedAlbumsQuery, List<RemovedAlbumDto>>,
                AlbumQueryRepository
            >();

            services.AddScoped<
                IQueryRepository<AlbumImagesQuery, List<AlbumImageDto>>,
                ImageQueryRepository
            >();
            services.AddScoped<
                IQueryRepository<RemovedImagesQuery, List<RemovedImageDto>>,
                ImageQueryRepository
            >();
            services.AddScoped<
                IQueryRepository<DetailedImageQuery, DetailedImage?>,
                ImageQueryRepository
            >();
            services.AddScoped<IQueryRepository<TagsQuery, List<TagDto>>, TagQueryRepository>();

            services.AddScoped<IImageAvailabilityChecker, ImageAvailabilityChecker>();
            services.AddSingleton<IImageStorageManager, ImageStorageManager>();
            services.AddSingleton<ICompressProcessor, CompressProcessor>();

            return services;
        }
    }
}
