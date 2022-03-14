using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    #nullable disable
    public class WeatherForecastsViewModel : ViewModelBase
    {
        private readonly IWeatherForecastService? weatherForecastService;

        public string ActionType = string.Empty;

        private ViewModelCollectionBase<WeatherForecastViewModel> forecasts;
        public ViewModelCollectionBase<WeatherForecastViewModel> Forecasts
        {
            get => forecasts;
            set
            {
                SetProperty(ref forecasts, value, nameof(Forecasts));
            }
        }

        public WeatherForecastsViewModel(IWeatherForecastService? weatherForecastService)
        {
            this.weatherForecastService = weatherForecastService;
        }

        public override async Task OnInitializedAsync()
        {
            // Simulate loading time
            await Task.Delay(1500);

            IEnumerable<WeatherForecastModel> forecasts = await weatherForecastService?.GetForecasts()!;
            IEnumerable<WeatherForecastViewModel> weatherForecastViewModels = forecasts.Select(x => new WeatherForecastViewModel(x));
            this.forecasts = new ViewModelCollectionBase<WeatherForecastViewModel>(weatherForecastViewModels);
            //this.forecasts.CollectionChanged
        }

        public void AddForecast()
        {
            WeatherForecastModel weatherForecast = new WeatherForecastModel(new DateTime(2017, 05, 15), 25, "Average");
            WeatherForecastViewModel viewModel = new WeatherForecastViewModel(weatherForecast);
            this.forecasts.Add(viewModel);
        }

        public void RandomizeData()
        {
            var random = new Random();

            foreach (WeatherForecastViewModel viewModel in forecasts!)
            {
                viewModel.BeginUpdate();
                viewModel.TemperatureC = random.Next(10, 40);
                viewModel.EndUpdate();
            }
        }

        public void OnForecastsChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            this.ActionType = args.Action.ToString();
        }

        /*
        public void RandomizeData()
        {
            var random = new Random();
            int i = 10;

            foreach (WeatherForecastViewModel weatherForecastViewModel in Forecasts!)
            {
                int temperatureC = random.Next((10 + i/3), 40 + i++);
                weatherForecastViewModel.Update()

                WeatherForecastModel model = weatherForecastViewModel.WeatherForecast;
                model.BeginUpdate();
                model.Date = weatherForecastViewModel.Date;
                model.TemperatureC = random.Next((10 + i/3), 40 + i++);
                model.Summary = weatherForecastViewModel.Summary;
                model.UpdateModel(model);
            }

            this.InformationSource = "Channel 3 News";
        }*/
    }
}
