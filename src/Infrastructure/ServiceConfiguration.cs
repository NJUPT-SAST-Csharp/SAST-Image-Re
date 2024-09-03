using System.Data.Common;
using Application;
using Application.AlbumServices;
using Application.AlbumServices.Queries;
using Application.ImageServices;
using Application.ImageServices.Queries;
using Application.Query;
using Application.SharedServices;
using Domain;
using Domain.AlbumDomain.AlbumEntity;
using Domain.AlbumDomain.ImageEntity;
using Domain.AlbumDomain.Services;
using Domain.Command;
using Domain.Core.Event;
using Domain.Extensions;
using Domain.UserDomain.UserEntity;
using Infrastructure.AlbumServices.Application;
using Infrastructure.AlbumServices.Domain;
using Infrastructure.Database;
using Infrastructure.ImageServices.Application;
using Infrastructure.SharedServices.EventBus;
using Infrastructure.SharedServices.Storage;
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
            services
                .AddScoped<IRepository<Album, AlbumId>, AlbumDomainRepository>()
                .AddScoped<IRepository<AlbumModel, AlbumId>, AlbumModelRepository>()
                .AddScoped<IQueryRepository<AlbumsQuery, List<AlbumDto>>, AlbumQueryRepository>()
                .AddScoped<
                    IQueryRepository<DetailedAlbumQuery, DetailedAlbum?>,
                    AlbumQueryRepository
                >()
                .AddScoped<
                    IQueryRepository<RemovedAlbumsQuery, List<RemovedAlbumDto>>,
                    AlbumQueryRepository
                >()
                .AddScoped<
                    IRepository<SubscribeModel, (AlbumId, UserId)>,
                    SubscribeModelRepository
                >();

            services
                .AddScoped<IAlbumAvailabilityChecker, AlbumAvailabilityChecker>()
                .AddScoped<ICategoryExistenceChecker, CategoryExistenceChecker>()
                .AddScoped<IAlbumTitleUniquenessChecker, AlbumTitleUniquenessChecker>()
                .AddScoped<ICollaboratorsExistenceChecker, CollaboratorsExistenceChecker>()
                .AddSingleton<ICoverStorageManager, CoverStorageManager>();

            return services;
        }

        public static IServiceCollection AddImageServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<ImageModel, ImageId>, ImageModelRepository>();
            services.AddScoped<IRepository<LikeModel, (ImageId, UserId)>, LikeModelRepository>();

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

            services.AddScoped<IImageAvailabilityChecker, ImageAvailabilityChecker>();
            services.AddSingleton<IImageStorageManager, ImageStorageManager>();
            services.AddSingleton<ICompressProcessor, CompressProcessor>();

            return services;
        }
    }
}
