using Norexia.Core.Application;
using Norexia.Core.Infrastructure;
using Norexia.Core.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOpenApiDocument((settings) =>
{
    settings.Title = "Norexia gestion commerciale (Facade API)";
    settings.Version = "v0";
    settings.DocumentName = "corev0";
});
builder.Services.AddSwaggerGen();
builder.Services.AddCors(policy =>
{
    policy.AddDefaultPolicy(builder => builder.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseOpenApi((config) =>
{
    config.DocumentName = "corev0";

});
app.UseSwaggerUi3();



app.UseDeveloperExceptionPage();
//app.UseMigrationsEndPoint();

if (!IsInvokedByNSwag())
{
    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
    }
}
//}


app.UseCors();
app.UseExceptionHandlingMiddleware();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

Console.WriteLine(app.Environment.EnvironmentName);

app.Run();


static bool IsInvokedByNSwag()
{
    var env = Environment.GetEnvironmentVariable("NSwag");
    return env is not null && bool.TrueString.Equals(env);
}