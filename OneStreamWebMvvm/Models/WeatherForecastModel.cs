using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class WeatherForecastModel : ModelBase
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        public string? Summary { get; set; }

        public WeatherForecastModel() { }
        public WeatherForecastModel(DateTime dateTime, int temperatureC, string? summary)
        {
            this.Date = Date;
            this.TemperatureC = temperatureC;
            this.Summary = summary;
        }

        public override void UpdateModel(object model)
        {
            this.TemperatureC = ((WeatherForecastModel)model).TemperatureC;
            this.Summary = ((WeatherForecastModel)model).Summary;
            ModelChanged?.Invoke(this, IsBatchUpdate);
        }
    }
}
