using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WeatherForecast.WebApi.Models;
using WeatherForecast.WebApi.Services;

namespace WebAppi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private HttpClient HttpClient;

        public IWeatherService WeatherService { get; }

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService, HttpClient httpClient)
        {
            _logger = logger;
            WeatherService = weatherService;
            HttpClient = httpClient;
        }

        [HttpPost("{postalCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromRoute] int postalCode)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var item = WeatherData.IleDeFranceWeatherData.SingleOrDefault(w => w.PostalCode == postalCode);

                if (item == null)
                    return NotFound(postalCode);

                // HTTP call to external weather API
                var response = await HttpClient.GetAsync(item.Url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var weatherData = JsonSerializer.Deserialize<WeatherForecastForCreationDto>(content, options);

                if (weatherData?.CurrentWeather != null)
                {
                    weatherData.Department = item.Department;
                    weatherData.DepartmentCode = item.DepartmentCode;
                    weatherData.PostalCode = item.PostalCode;
                    weatherData.City = item.City;

                    _logger.LogInformation($"Température actuelle à {item.City} : {weatherData.CurrentWeather.Temperature}°C");

                    await WeatherService.Add(weatherData);
                }
                else
                {
                    _logger.LogInformation("Les données météo n'ont pas pu être récupérées.");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var result = await WeatherService.Get();

            return Ok(result);
        }

        [HttpGet("{locationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] int locationId)
        {
            var result = await WeatherService.Get(locationId);
            return Ok(result);
        }
    }
}