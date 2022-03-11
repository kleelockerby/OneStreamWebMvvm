﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class ParameterCache : IParameterCache
    {
        private readonly ConcurrentDictionary<Type, ParameterInfo> cache = new();

        public ParameterInfo? Get(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            return cache.TryGetValue(type, out var info) ? info : null;
        }

        public void Set(Type type, ParameterInfo info)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            cache[type] = info ?? throw new ArgumentNullException(nameof(info));
        }
    }
}