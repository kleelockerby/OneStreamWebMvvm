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
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected bool IsBatchUpdate = false;

        public IServiceProvider ServiceProvider { get; set; } = null!;

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

        protected bool SetProperty<TItem>(ref TItem field, TItem value, Action<TItem> callback, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<TItem>.Default.Equals(field, value))
            {
                field = value;
                callback(value);
                if (!IsBatchUpdate)
                {
                    this.OnPropertyChanged(propertyName!); 
                }
                return true;
            }           
            return false;
        }

        protected void SetProperty<TItem>(TItem value, [CallerMemberName] string? propertyName = null)
        {
            if (!IsBatchUpdate)
            {
                this.OnPropertyChanged(propertyName!);
            }
        }

        protected void SetProperty<TItem>(TItem value, Action<TItem> callback, [CallerMemberName] string? propertyName = null)
        {
            callback?.Invoke(value);
            if (!IsBatchUpdate)
            {
                this.OnPropertyChanged(propertyName!);
            }
        }

        public bool SetProperty<TModel, TItem>(TItem field, TItem value, TModel model, Action<TModel, TItem> callback, [CallerMemberName] string? propertyName = null) where TModel : class
        {
            if (!EqualityComparer<TItem>.Default.Equals(field, value))
            {
                field = value;
                callback(model, value);
                this.OnPropertyChanged(propertyName);
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

        protected internal virtual void OnInitialized() { }
        
        protected internal virtual Task OnInitializedAsync()
        {
            return Task.CompletedTask;
        }

        protected internal virtual void OnParametersSet() { }

        protected internal virtual Task OnParametersSetAsync()
        {
            return Task.CompletedTask;
        }

        protected internal void StateHasChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        protected internal virtual bool ShouldRender()
        {
            return true;
        }

        protected internal virtual void OnAfterRender(bool firstRender) { }

        protected internal virtual Task OnAfterRenderAsync(bool firstRender)
        {
            return Task.CompletedTask;
        }

        protected internal virtual Task SetParametersAsync(ParameterView parameters)
        {
            return Task.CompletedTask;
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