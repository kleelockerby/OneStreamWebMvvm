using System;
using System.Collections.Generic;
using System.Reflection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    internal interface IParameterResolver
    {
        ParameterInfo ResolveParameters(Type componentType, Type viewModelType);
    }
}
