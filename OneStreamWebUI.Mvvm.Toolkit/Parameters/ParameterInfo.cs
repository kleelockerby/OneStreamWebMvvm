using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    internal record ParameterInfo
    {
        private readonly Dictionary<PropertyInfo, PropertyInfo> parameters = new();

        public IReadOnlyDictionary<PropertyInfo, PropertyInfo> Parameters => parameters;

        public ParameterInfo(IEnumerable<PropertyInfo> componentProperties, IEnumerable<PropertyInfo> viewModelProperties)
        {
            var viewModelPropDict = viewModelProperties.ToDictionary(x => x.Name);

            foreach (var componentProperty in componentProperties)
            {
                if (!viewModelPropDict.TryGetValue(componentProperty.Name, out var viewModelProperty))
                {
                    continue;
                }

                parameters.Add(componentProperty, viewModelProperty);
            }
        }
    }
}