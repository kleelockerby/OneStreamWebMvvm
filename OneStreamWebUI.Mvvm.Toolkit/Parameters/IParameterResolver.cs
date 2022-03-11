using System;
using System.Collections.Generic;
using System.Reflection;

namespace OneStreamWebUI.Mvvm
{
    public interface IParameterResolver
    {
        IEnumerable<PropertyInfo> ResolveParameters(Type memberType);
    }
}
