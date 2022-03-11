using System;
using System.Runtime.Serialization;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    internal class BindingException : Exception
    {
        public BindingException() { }

        public BindingException(string message) : base(message) { }
    }
}
