using Microsoft.Extensions.Options;
using System.Net;
using Weather.Api.Models;

namespace Weather.Api.Services;

public class WeatherService : IWeatherService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptionsMonitor<OpenWeatherApiSettings> _weatherApiOptions;

    //IOptionsMonitor ==> ako je supportirano od configuraiton providera, može updejtati settingse u sklopu runtima
    public WeatherService(IHttpClientFactory httpClientFactory,
        IOptionsMonitor<OpenWeatherApiSettings> weatherApiOptions)
    {
        _httpClientFactory = httpClientFactory;
        _weatherApiOptions = weatherApiOptions;
    }

    public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_weatherApiOptions.CurrentValue.ApiKey}&units=metric";
        var httpClient = _httpClientFactory.CreateClient();

        var weatherResponse = await httpClient.GetAsync(url);
        if (weatherResponse.StatusCode == HttpStatusCode.NotFound || weatherResponse.StatusCode == HttpStatusCode.Unauthorized)
        {
            return null;
        }

        var weather = await weatherResponse.Content.ReadFromJsonAsync<WeatherResponse>();
        return weather;
    }
}
