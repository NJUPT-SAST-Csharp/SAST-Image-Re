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
using Infrastructure.Application.AlbumServices;
using Infrastructure.Application.ImageSerivces;
using Infrastructure.Application.ImageServices;
using Infrastructure.Database;
using Infrastructure.Domain.AlbumServices;
using Infrastructure.EventBus;
using Infrastructure.Storage;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddDb(
            this IServiceCollection services,
            string connectionString
        )
        {
            services
                .AddScoped<DbConnection>(_ => new NpgsqlConnection(connectionString))
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

            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.NotificationPublisherType = typeof(ForeachAwaitPublisher);
                options.RegisterServicesFromAssemblies(
                    DomainAssembly.Assembly,
                    ApplicationAssembly.Assembly
                );
                options.AutoRegisterRequestProcessors = true;
                options.AddOpenRequestPostProcessor(typeof(UnitOfWorkPostProcessor<,>));
            });

            services.AddScoped<IQueryRequestSender, QuerySender>();
            services.AddScoped<IDomainCommandSender, CommandSender>();
            services.AddScoped<IDomainEventPublisher, EventPublisher>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Album, AlbumId>, AlbumDomainRepository>();
            services.AddScoped<IImageTagsExistenceChecker, ImageTagsExistenceChecker>();
            services.AddScoped<ICategoryExistenceChecker, CategoryExistenceChecker>();
            services.AddScoped<ICollaboratorsExistenceChecker, CollaboratorsExistenceChecker>();
            services.AddScoped<IAlbumTitleUniquenessChecker, AlbumTitleUniquenessChecker>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository<AlbumModel, AlbumId>, AlbumModelRepository>();
            services.AddScoped<IRepository<ImageModel, ImageId>, ImageModelRepository>();

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

            services.AddScoped<IAlbumAvailabilityChecker, AlbumAvailabilityChecker>();

            services.AddSingleton<IImageStorageManager, ImageStorageManager>();
            services.AddSingleton<IAlbumCoverManager, AlbumCoverManager>();

            return services;
        }

        public static IServiceCollection ConfigureStorage(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.Configure<StorageOptions>(configuration);
            services.AddSingleton<IStorageManager, StorageManager>();

            return services;
        }
    }
}
