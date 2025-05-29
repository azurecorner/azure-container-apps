using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.OpenApi.Models;
using WeatherForecast.Infrastructure;
using WeatherForecast.WebApi;
using WeatherForecast.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<ITelemetryInitializer>(new CloudRoleNameTelemetryInitializer("WeatherForecast.WebApi")); 
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"];
});

builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services.RegisterInfrastureDependencies(builder.Configuration);
builder.Services.AddHttpClient();
//logging

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "LogisticManagement API",
        Version = "v1"
    });
});

var app = builder.Build();
app.UsePathBase("/webapi");
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors();
app.MapControllers();

app.Run();