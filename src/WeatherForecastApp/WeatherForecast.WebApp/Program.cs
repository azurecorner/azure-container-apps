var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("WebApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WEBAPI_URL"] ?? throw new InvalidOperationException("WEBAPI_URL configuration is missing or empty."));
});

//if (!builder.Environment.IsDevelopment())
//{
//    builder.Services.AddDataProtection()
//             .PersistKeysToFileSystem(new DirectoryInfo(@"/mnt/dataprotectionkeys/"))
//             .SetApplicationName("WeatherForecastApp");

//}


var app = builder.Build();

// Configure the HTTP request pipeline.

 app.UseExceptionHandler("/Home/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

app.UsePathBase("/webapp");


app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();