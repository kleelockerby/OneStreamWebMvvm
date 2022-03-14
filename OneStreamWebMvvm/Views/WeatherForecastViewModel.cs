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
        //private WeatherForecastModel model;
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
            //this.model = weatherForecastModel;
            this.date = weatherForecastModel.Date;
            TemperatureC = weatherForecastModel.TemperatureC;
            this.summary = weatherForecastModel.Summary;

            //weatherForecastModel.ModelChanged = UpdateItem;
        }

        public void Add(DateTime date, int tempC, string? summary)
        {
            //WeatherForecastModel model = new WeatherForecastModel(date, tempC, summary);
            this.date = date;
            this.TemperatureC = tempC;
            this.summary = summary;
        }

        private void TempValueChanged(int tempC)
        {
            temperatureC = tempC;
            temperatureF = (int)(tempC/0.5556) + 32;
        }




        /*public void UpdateItem(object objForecast, bool isBatchUpdate)
        {
            WeatherForecastModel? forecast = objForecast as WeatherForecastModel;
            if (isBatchUpdate)
            {
                this.temperatureC = forecast?.TemperatureC;
                this.temperatureF = forecast?.TemperatureF;
            }
            else
            {
                this.TemperatureC = forecast?.TemperatureC;
                this.TemperatureF = forecast?.TemperatureF;
            }
        }*/
    }
}