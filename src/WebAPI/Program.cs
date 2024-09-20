using System.Text.Json;
using System.Text.Json.Serialization;
using Infrastructure;
using WebAPI.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddUserServices(builder.Configuration).AddJwtAuth(builder.Configuration);
builder.Services.AddAlbumServices();
builder.Services.AddImageServices();
builder.Services.AddTagServices();

builder.Services.AddExceptionHandlers();

builder.Logging.AddLogger();

builder.Services.AddResponseCaching();

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.WriteAsString;
    });

var app = builder.Build();

app.UseCors(cors =>
    cors.AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).AllowCredentials()
);

app.UseResponseCaching();

app.UseExceptionHandler(op => { });

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
