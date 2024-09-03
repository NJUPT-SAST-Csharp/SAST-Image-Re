using System.Text.Json;
using System.Text.Json.Serialization;
using Infrastructure;
using WebAPI.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddAlbumServices();
builder.Services.AddImageServices();

builder.Services.AddExceptionHandlers();

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.WriteAsString;
    });

var app = builder.Build();

app.UseExceptionHandler(op => { });

//app.EnsureDatabase();

app.UseAuthorization();

app.MapControllers();

app.Run();
