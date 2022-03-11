using System.Collections.Concurrent;
using System;
using System.Collections.Generic;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    internal interface IParameterCache
    {
        ParameterInfo? Get(Type type);
        void Set(Type type, ParameterInfo info);
    }

    internal class ParameterCache : IParameterCache
    {
        private readonly ConcurrentDictionary<Type, ParameterInfo> cache = new();

        public ParameterInfo? Get(Type type)
        {
            return cache.TryGetValue(type, out var info) ? info : null;
        }

        public void Set(Type type, ParameterInfo info)
        {
            cache[type] = info ?? throw new ArgumentNullException(nameof(info));
        }
    }
}