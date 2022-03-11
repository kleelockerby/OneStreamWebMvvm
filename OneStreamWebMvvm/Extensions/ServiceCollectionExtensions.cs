﻿using System;
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
            //serviceCollection.AddTransient<WeatherForecastItemViewModel>();
            return serviceCollection;
        }
        
    }
}
