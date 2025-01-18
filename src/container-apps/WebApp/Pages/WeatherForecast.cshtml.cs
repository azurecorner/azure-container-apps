using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages
{
    public class WeatherForecastModel : PageModel
    {
        public string WeatherApiUrl { get; set; }

        public WeatherForecastModel(IConfiguration configuration)
        {
            Console.WriteLine("ASPNETCORE_ENVIRONMENT =", configuration["ASPNETCORE_ENVIRONMENT"]);
            // Retrieve the API URL from the environment variable or app settings
            //WeatherApiUrl = configuration["WebApiUrl"] ?? throw new ArgumentNullException("WebApiUrl is not set in the configuration file");
            WeatherApiUrl = "https://datasync-aca-api.proudmeadow-430e00ae.westeurope.azurecontainerapps.io/webapi/api/WeatherForecast";
        }

        public void OnGet()
        {
        }
    }
}