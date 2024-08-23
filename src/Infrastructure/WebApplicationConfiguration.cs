using Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class WebApplicationConfiguration
    {
        public static void EnsureDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var domainDbContext = scope.ServiceProvider.GetRequiredService<DomainDbContext>();
            var queryDbContext = scope.ServiceProvider.GetRequiredService<QueryDbContext>();

            Task.WhenAll(
                    domainDbContext.Database.MigrateAsync(),
                    queryDbContext.Database.MigrateAsync()
                )
                .Wait();
        }
    }
}
