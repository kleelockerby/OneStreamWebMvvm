using System.Net.Http.Json;

namespace OneStreamWebMvvm
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly HttpClient httpClient;

        public WeatherForecastService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<IEnumerable<WeatherForecastModel>> GetForecasts()
        {
            return httpClient.GetFromJsonAsync<IEnumerable<WeatherForecastModel>>("sample-data/weather.json")!;
        }
    }
}
