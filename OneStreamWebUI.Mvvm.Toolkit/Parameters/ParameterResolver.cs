using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class ParameterResolver : IParameterResolver
    {
        public IEnumerable<PropertyInfo> ResolveParameters(Type memberType)
        {
            if (memberType == null)
            {
                throw new ArgumentNullException(nameof(memberType));
            }

            var componentProperties = memberType.GetProperties();
            var resolvedComponentProperties = new List<PropertyInfo>();
            foreach (var componentProperty in componentProperties)
            {
                if (componentProperty.GetSetMethod() is null)
                {
                    continue;
                }

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