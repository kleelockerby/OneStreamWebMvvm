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
            serviceCollection.AddTransient<ShoppingCartItemViewModel>();
            serviceCollection.AddTransient<ShoppingCartViewModel>();
            serviceCollection.AddTransient<MessageViewModel>();
            serviceCollection.AddTransient<MessageListenerViewModel>();
            return serviceCollection;
        }

        public static IServiceCollection AddComponentServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IWeatherForecastService, WeatherForecastService>();
            serviceCollection.AddSingleton<IOrdersService, OrdersService>();
            serviceCollection.AddSingleton<IProductService, ProductService>();
            serviceCollection.AddSingleton<ICustomerService, CustomerService>();
            serviceCollection.AddSingleton<IProductRepository, ProductRepository>();
            serviceCollection.AddSingleton<ICartItemService, CartItemService>();
            return serviceCollection;
        }
    }
}
