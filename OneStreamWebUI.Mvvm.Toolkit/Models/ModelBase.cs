using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        protected bool IsBatchUpdate = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetProperty<TItem>(ref TItem field, TItem value, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<TItem>.Default.Equals(field, value))
            {
                field = value;
                if (!IsBatchUpdate)
                {
                    OnPropertyChanged(propertyName!);
                }
                return true;
            }
            return false;
        }

        public bool SetProperty<TItem>(TItem oldValue, TItem newValue, Action<TItem, TItem> callback, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<TItem>.Default.Equals(oldValue, newValue))
            {
                callback(oldValue, newValue);
                if (!IsBatchUpdate)
                {
                    this.OnPropertyChanged(propertyName!);
                }
                return true;
            }
            return false;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged == null)
            {
                return;
            }
            propertyChanged((object)this, e);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void BeginUpdate()
        {
            this.IsBatchUpdate = true;
        }

        public void EndUpdate()
        {
            this.IsBatchUpdate = false;
        }
    }
}
