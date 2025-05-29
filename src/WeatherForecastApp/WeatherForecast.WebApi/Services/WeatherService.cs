using LogisticManagement.Infrastructure.Repositories;
using WeatherForecast.WebApi.Models;
using WeatherForecast.WebApi.Models.Creation;
using WeatherForecast.WebApi.WeatherForecast.Application.Mappers;

namespace WeatherForecast.WebApi.Services;

public class WeatherService : IWeatherService
{
    private IWeatherRepository WeatherRepository { get; }

    public WeatherService(IWeatherRepository weatherRepository)
    {
        WeatherRepository = weatherRepository;
    }

    public async Task Add(WeatherForecastForCreationDto weatherDto)
    {
        // Conversion des données du DTO
        var location = weatherDto.ToLocation();

        // Appel à la base de données
        await WeatherRepository.Add(location, location.Weather);
    }

    public async Task<List<WeatherForecastInListDto>> Get()
    {
        var result = await WeatherRepository.GetAll();
        return result.Select(x => x.ToListDto()).ToList();
    }

    public async Task<WeatherForecastInListDto> Get(int locationId)
    {
        var result = await WeatherRepository.Get(locationId);

        if (result == null)
        {
            throw new ArgumentNullException(nameof(result), "Location not found for the given locationId.");
        }

        return result.ToListDto();
    }
}