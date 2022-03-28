using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace OneStreamWebMvvm
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddViewModels(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<CounterViewModel>();
            serviceCollection.AddTransient<ClockViewModel>();
            serviceCollection.AddTransient<ParametersViewModel>();
            serviceCollection.AddTransient<WeatherForecastViewModel>();
            serviceCollection.AddTransient<WeatherForecastsViewModel>();
            serviceCollection.AddTransient<OrderViewModel>();
            serviceCollection.AddTransient<OrdersViewModel>();
            serviceCollection.AddTransient<OrdersGridViewModel>();
            serviceCollection.AddTransient<OrdersGridModelViewModel>();
            serviceCollection.AddTransient<OrdersComponentViewModel>();
            serviceCollection.AddTransient<OrderComponentViewModel>();
            return serviceCollection;
        }

        public static IServiceCollection AddComponentServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IWeatherForecastService, WeatherForecastService>();
            serviceCollection.AddSingleton<IOrdersService, OrdersService>();
            return serviceCollection;
        }
    }
}
