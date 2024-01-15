using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.Infrastructure;
using Norexia.Core.Infrastructure.Persistence;
using Norexia.Core.Infrastructure.Persistence.Interceptors;
using Norexia.Core.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(options =>

           options.UseNpgsql(configuration.GetConnectionString("NorexiaDataBase"),
           builer =>
           {
               builer.MigrationsHistoryTable("__MigrationsHistory", "app");
               builer.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
           }));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IFileServices, FileServices>();

        return services;
    }
}

