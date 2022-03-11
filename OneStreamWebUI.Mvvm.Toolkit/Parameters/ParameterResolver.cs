using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    internal class ParameterResolver : IParameterResolver
    {
        private readonly IParameterCache parameterCache;

        public ParameterResolver(IParameterCache parameterCache)
        {
            parameterCache = parameterCache;
        }

        public ParameterInfo ResolveParameters(Type componentType, Type viewModelType)
        {
            var parameterInfo = parameterCache.Get(componentType);
            if (parameterInfo is not null)
            {
                return parameterInfo;
            }

            var componentParameters = ResolveTypeParameters(componentType);
            var viewModelParameters = ResolveTypeParameters(viewModelType);

            parameterInfo = new ParameterInfo(componentParameters, viewModelParameters);
            parameterCache.Set(componentType, parameterInfo);

            return parameterInfo;
        }

        private static IEnumerable<PropertyInfo> ResolveTypeParameters(Type memberType)
        {
            var componentProperties = memberType.GetProperties();
            var resolvedComponentProperties = new List<PropertyInfo>();

            foreach (var componentProperty in componentProperties)
            {
                // Skip if property has no public setter
                if (componentProperty.GetSetMethod() is null)
                {
                    continue;
                }

                // If the property is marked as a parameter add it to the list
                ParameterAttribute? parameterAttribute = componentProperty.GetCustomAttribute<ParameterAttribute>();
                if (parameterAttribute != null)
                {
                    resolvedComponentProperties.Add(componentProperty);
                }
            }

            return resolvedComponentProperties;
        }
    }
}