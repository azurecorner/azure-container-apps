using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using WeatherForecast.Infrastructure.Models;
using WeatherForecast.WebApp.Models;

namespace WeatherForecast.WebApp.Controllers
{
    public class WeatherForecastController : Controller
    {
        public ILogger<WeatherForecastController> Logger { get; }

            private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherForecastController> _logger;
 
        public WeatherForecastController(IHttpClientFactory clientFactory, ILogger<WeatherForecastController> logger)
        {
            _httpClient = clientFactory.CreateClient("WebApi");
            _logger = logger;
     
        }

        // GET: WeatherForecastController
        public async Task<ActionResult> Index()
        {
            

            try
            {
                var response = await _httpClient.GetAsync("WeatherForecast");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to retrieve weather data. Status code: {StatusCode}", response.StatusCode);
                    return StatusCode((int)response.StatusCode);
                }

                var content = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var weatherData = JsonSerializer.Deserialize<IEnumerable<LocationInListViewModel>>(content, options);

                _logger.LogInformation("Successfully fetched weather data. Items count: {Count}", weatherData?.Count() ?? 0);

                return View(weatherData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while fetching weather data.");
             
                return StatusCode(500, "Internal server error");
            }
        }


        // GET: WeatherForecastController/Details/Id
        public async Task<ActionResult> Details(int Id)
        {
            var response = await _httpClient.GetAsync($"WeatherForecast/{Id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound(); // ou afficher une vue d’erreur personnalisée
            }

            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var weatherData = JsonSerializer.Deserialize<LocationInListViewModel>(content, options);

            if (weatherData == null)
            {
                return BadRequest("Invalid data received from the API.");
            }

            return View(weatherData);
        }

        // GET: WeatherForecastController/Create
        public ActionResult Create()
        {
            var model = new WeatherForecastCreateViewModel
            {
                PostalCodeOptions = GetPostalCodeSelectList()
            };
            return View(model);
        }

        // POST: WeatherForecastController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(WeatherForecastCreateViewModel model)
        {
            model.PostalCodeOptions = GetPostalCodeSelectList();

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"WeatherForecast/{model.PostalCode}", content);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        private IEnumerable<SelectListItem> GetPostalCodeSelectList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Select...", Value = "", Disabled = true, Selected = true },
                new SelectListItem { Text = "75000 - Paris", Value = "75000" },
                new SelectListItem { Text = "91000 - Essonne (Évry-Courcouronnes)", Value = "91000" },
                new SelectListItem { Text = "92000 - Hauts-de-Seine (Nanterre)", Value = "92000" },
                new SelectListItem { Text = "93000 - Seine-Saint-Denis (Bobigny)", Value = "93000" },
                new SelectListItem { Text = "94000 - Val-de-Marne (Créteil)", Value = "94000" },
                new SelectListItem { Text = "95000 - Val-d'Oise (Cergy)", Value = "95000" },
                new SelectListItem { Text = "77000 - Seine-et-Marne (Melun)", Value = "77000" },
                new SelectListItem { Text = "78000 - Yvelines (Versailles)", Value = "78000" }
            };
        }
    }
}