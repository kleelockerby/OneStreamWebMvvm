using System;
using System.Collections.Generic;
using System.Reflection;

namespace OneStreamWebUI.Mvvm
{
    internal interface IParameterResolver
    {
        IEnumerable<PropertyInfo> ResolveParameters(Type memberType);
    }
}
