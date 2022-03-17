using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class WeatherForecastModel
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public string? Summary { get; set; }

        public WeatherForecastModel() { }
        public WeatherForecastModel(DateTime dateTime, int temperatureC, string? summary)
        {
            this.Date = dateTime;
            this.TemperatureC = temperatureC;
            this.Summary = summary;
        }
    }
}
