namespace Norexia.Core.WebApi.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(
   this IApplicationBuilder app)
   => app.UseMiddleware<ExceptionHandlingMiddleware>();
}
