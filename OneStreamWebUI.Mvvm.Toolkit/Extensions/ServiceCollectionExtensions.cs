using Microsoft.Extensions.DependencyInjection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection OneStreamMvvm(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IBindingFactory, BindingFactory>();
            serviceCollection.AddSingleton<IParameterResolver, ParameterResolver>();
            serviceCollection.AddSingleton<IParameterCache, ParameterCache>();
            serviceCollection.AddSingleton<IViewModelParameterSetter, ViewModelParameterSetter>();            
            serviceCollection.AddSingleton<IMessageAggregator, MessageAggregator>();
            serviceCollection.AddTransient<IWeakEventManager, WeakEventManager>();
            serviceCollection.AddHttpContextAccessor();
            return serviceCollection;
        }
    }
}