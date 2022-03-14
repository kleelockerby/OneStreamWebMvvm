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
                //field = value;
                callback(value);
                if (!IsBatchUpdate)
                {
                    this.OnPropertyChanged(propertyName!); 
                }
                return true;
            }           
            return false;
        }

        protected bool SetProperty<TModel, TItem>(ref TItem field, TItem value, TModel model, Action<TModel, TItem> callback, [CallerMemberName] string? propertyName = null) where TModel : class
        {
            if (!EqualityComparer<TItem>.Default.Equals(field, value))
            {
                callback(model, value);
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