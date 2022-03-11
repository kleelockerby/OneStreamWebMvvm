using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public interface IViewModelParameterSetter
    {
        void ResolveAndSet(ComponentBase component, ViewModelBase viewModel);
    }

    internal class ViewModelParameterSetter : IViewModelParameterSetter
    {
        private readonly IParameterResolver parameterResolver;

        public ViewModelParameterSetter(IParameterResolver parameterResolver)
        {
            parameterResolver = parameterResolver;
        }

        public void ResolveAndSet(ComponentBase component, ViewModelBase viewModel)
        {
            var componentType = component.GetType();
            var viewModelType = viewModel.GetType();

            var parameterInfo = parameterResolver.ResolveParameters(componentType, viewModelType);
            foreach (var (componentProperty, viewModelProperty) in parameterInfo.Parameters)
            {
                var value = componentProperty.GetValue(component);
                var parameterTypeDiffers = componentProperty.PropertyType != viewModelProperty.PropertyType;
                if (value != null && parameterTypeDiffers)
                {
                    value = ConvertValue(componentProperty.PropertyType, viewModelProperty.PropertyType, value);
                }

                viewModelProperty.SetValue(viewModel, value);
            }
        }

        private static object? ConvertValue(Type componentType, Type viewModelType, object value)
        {
            var converter = TypeDescriptor.GetConverter(viewModelType);
            return converter.CanConvertFrom(componentType) ? converter.ConvertTo(value, viewModelType) : value;
        }
    }
}