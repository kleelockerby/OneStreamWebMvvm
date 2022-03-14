namespace OneStreamWebMvvm
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecastModel>>? GetForecasts();
    }
}
