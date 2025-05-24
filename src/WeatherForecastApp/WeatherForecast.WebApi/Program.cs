using Microsoft.OpenApi.Models;
using WeatherForecast.Infrastructure;
using WeatherForecast.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var meterName = "OtelReferenceApp.WeatherForecast";


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