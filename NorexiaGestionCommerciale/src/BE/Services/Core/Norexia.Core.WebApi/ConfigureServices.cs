using Norexia.Core.Application.Common.Interfaces;
using Norexia.Core.WebApi.Middleware;
using Norexia.Core.WebApi.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        //services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        services.AddTransient<ExceptionHandlingMiddleware>();

        return services;
    }
}
