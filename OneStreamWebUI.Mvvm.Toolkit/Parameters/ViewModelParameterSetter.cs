using System;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class ViewModelParameterSetter : IViewModelParameterSetter
    {
        private readonly IParameterCache parameterCache;
        private readonly IParameterResolver parameterResolver;

        public ViewModelParameterSetter(IParameterResolver iparameterResolver, IParameterCache iparameterCache)
        {
            parameterResolver = iparameterResolver ?? throw new ArgumentNullException(nameof(iparameterResolver));
            parameterCache = iparameterCache ?? throw new ArgumentNullException(nameof(iparameterCache));
        }

        public void ResolveAndSet(ComponentBase component, ViewModelBase viewModel)
        {
            if (component == null) throw new ArgumentNullException(nameof(component));
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            Type? componentType = component.GetType();

            ParameterInfo? parameterInfo = parameterCache.Get(componentType);
            if (parameterInfo == null)
            {
                var componentParameters = parameterResolver.ResolveParameters(componentType);
                var viewModelParameters = parameterResolver.ResolveParameters(viewModel.GetType());
                parameterInfo = new ParameterInfo(componentParameters, viewModelParameters);
                parameterCache.Set(componentType, parameterInfo);
            }

            foreach ((PropertyInfo componentProperty, PropertyInfo viewModelProperty) in parameterInfo.Parameters)
            {
                var value = componentProperty.GetValue(component);
                viewModelProperty.SetValue(viewModel, value);
            }
        }
    }
}
