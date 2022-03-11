using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class ParameterInfo
    {
        private readonly Dictionary<PropertyInfo, PropertyInfo> parameters = new();

        public ParameterInfo(IEnumerable<PropertyInfo> componentProperties, IEnumerable<PropertyInfo> viewModelProperties)
        {
            if (componentProperties == null) throw new ArgumentNullException(nameof(componentProperties));
            if (viewModelProperties == null) throw new ArgumentNullException(nameof(viewModelProperties));

            Dictionary<string, PropertyInfo> viewModelPropDict = viewModelProperties.ToDictionary(x => x.Name);

            foreach (var componentProperty in componentProperties)
            {
                if (!viewModelPropDict.TryGetValue(componentProperty.Name, out var viewModelProperty))
                {
                    continue;
                }

                parameters.Add(componentProperty, viewModelProperty);
            }
        }

        public IReadOnlyDictionary<PropertyInfo, PropertyInfo> Parameters => parameters;
    }
}