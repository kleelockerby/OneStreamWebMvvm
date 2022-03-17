using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class WeatherForecastViewModel : ViewModelBase
    {
        private WeatherForecastModel weatherForecastModel;

        private DateTime date;
        private int temperatureC;
        private int? temperatureF;
        private string? summary;

        public DateTime Date => date;
        public string? Summary => summary;

        public int TemperatureC
        {
            get => temperatureC;
            set
            {
                SetProperty(ref temperatureC, value, TempValueChanged, nameof(TemperatureC));
            }
        }

        public int? TemperatureF
        {
            get => temperatureF;
        }

        public WeatherForecastViewModel(WeatherForecastModel weatherForecastModel)
        {
            this.weatherForecastModel = weatherForecastModel;
            this.date = this.weatherForecastModel.Date;
            TemperatureC = this.weatherForecastModel.TemperatureC;
            this.summary = this.weatherForecastModel.Summary;
        }

        private void TempValueChanged(int tempC)
        {
            temperatureF = (int)(tempC/0.5556) + 32;
            weatherForecastModel.TemperatureC = tempC;
        }
    }
}