using System;
using System.ComponentModel;
using System.Reflection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public interface IBinding : IDisposable
    {
        INotifyPropertyChanged Source { get; }
        PropertyInfo PropertyInfo { get; }
        event EventHandler? BindingValueChanged;
        void Initialize();
        object GetValue();
    }
}
