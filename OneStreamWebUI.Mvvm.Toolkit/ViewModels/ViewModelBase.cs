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
        private bool isBatchUpdate = false;

        public IServiceProvider ServiceProvider { get; set; } = null!;

        public event PropertyChangedEventHandler PropertyChanged;

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
                if (!isBatchUpdate)
                {
                    OnPropertyChanged(propertyName!);
                }
                return true;
            }
            return false;           
        }

        public bool SetProperty<TItem>(ref TItem field, TItem value, Action<TItem> callback, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<TItem>.Default.Equals(field, value))
            {
                field = value;
                callback(value);
                if (!isBatchUpdate)
                {
                    this.OnPropertyChanged(propertyName!); 
                }
                return true;
            }           
            return false;
        }

        public void SetProperty<TItem>(TItem value, [CallerMemberName] string? propertyName = null)
        {
            if (!isBatchUpdate)
            {
                this.OnPropertyChanged(propertyName!);
            }
        }

        public void SetProperty<TItem>(TItem value, Action<TItem> callback, [CallerMemberName] string? propertyName = null)
        {
            callback?.Invoke(value);
            if (!isBatchUpdate)
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

        public virtual void OnPropertyChanged(PropertyChangedEventArgs e)
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

        public void StateHasChanged()
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

        public void BeginUpdate()
        {
            this.isBatchUpdate = true;
        }

        public void EndUpdate()
        {
            this.isBatchUpdate = false;
        }
    }
}