using System;
using System.Collections.Generic;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    internal interface IParameterCache
    {
        ParameterInfo? Get(Type type);
        void Set(Type type, ParameterInfo info);
    }
}
