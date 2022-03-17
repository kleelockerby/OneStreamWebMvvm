using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public bool SetProperty<TModel, TItem>(ref TItem field, TItem value, TModel model, Action<TModel, string, TItem> callback, [CallerMemberName] string? propertyName = null) where TModel : class
        {
            if (!EqualityComparer<TItem>.Default.Equals(field, value))
            {
                field = value;
                callback?.Invoke(model, propertyName, value);
                this.OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        public bool SetProperty<TItem>(ref TItem field, TItem value, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<TItem>.Default.Equals(field, value))
            {
                this.OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
