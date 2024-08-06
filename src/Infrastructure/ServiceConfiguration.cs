using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddDb(
            this IServiceCollection services,
            string connectionString
        )
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(connectionString);
                options.UseSnakeCaseNamingConvention();
            });
            return services;
        }
    }
}
