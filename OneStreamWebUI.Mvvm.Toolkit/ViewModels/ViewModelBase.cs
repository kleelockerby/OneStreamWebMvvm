using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Collections.Specialized;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        public virtual void OnInitialized() { }
        public virtual Task OnInitializedAsync()
        {
            return Task.CompletedTask;
        }

        public bool SetProperty<TItem>(ref TItem field, TItem value, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<TItem>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        protected void SetProperty<TItem>(ref TItem field, TItem value, Action? onChanged = null, Action<TItem>? onChanging = null, [CallerMemberName] string propertyName = "Unspecified")
        {
            if (!EqualityComparer<TItem>.Default.Equals(field, value))
            {
                onChanging?.Invoke(value);
                OnPropertyChanging(new PropertyChangingEventArgs(propertyName));

                field = value;

                onChanged?.Invoke();
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }
            return;
        }

        protected bool SetProperty<TViewModel, TItem>(TItem oldValue, TItem newValue, TViewModel viewModel, Action<TViewModel, TItem> callback, [CallerMemberName] string? propertyName = null) where TViewModel : class
        {
            if (EqualityComparer<TItem>.Default.Equals(oldValue, newValue))
            {
                return false;
            }
            this.OnPropertyChanging(propertyName);
            callback(viewModel, newValue);
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
            if (propertyChanging == null)
            {
                return;
            }
            propertyChanging((object)this, e);
        }

        public virtual void OnPropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
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

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnParametersSet() { }

        public virtual Task OnParametersSetAsync()
        {
            return Task.CompletedTask;
        }

        protected void StateHasChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public virtual bool ShouldRender()
        {
            return true;
        }

        public virtual void OnAfterRender(bool firstRender) { }

        public virtual Task OnAfterRenderAsync(bool firstRender)
        {
            return Task.CompletedTask;
        }

        public virtual Task SetParametersAsync(ParameterView parameters)
        {
            return Task.CompletedTask;
        }
    }
}