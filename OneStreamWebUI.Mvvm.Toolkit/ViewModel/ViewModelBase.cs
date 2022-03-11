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
        private readonly Dictionary<string, List<Func<object, Task>>> subscriptions = new();

        public IServiceProvider RootServiceProvider { get; set; } = null!;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName!);
                if (!subscriptions.ContainsKey(propertyName!))
                {
                    return true;
                }

                foreach (var action in subscriptions[propertyName!]) action(value!);
                return true;
            }

            return false;
        }

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void Subscribe<T>(Expression<Func<T>>? expression, Action<T> action)
        {
            SubscribeAsync(
                expression,
                arg =>
                {
                    action(arg);
                    return Task.CompletedTask;
                }
            );
        }

        protected void SubscribeAsync<T>(Expression<Func<T>>? property, Func<T, Task> func)
        {
            if (property is null)
            {
                throw new BindingException("Property cannot be null");
            }

            if (!(property.Body is MemberExpression m))
            {
                throw new BindingException("Subscription member must be a property");
            }

            if (!(m.Member is PropertyInfo propertyInfo))
            {
                throw new BindingException("Subscription member must be a property");
            }

            var propertyName = propertyInfo.Name;
            if (!subscriptions.ContainsKey(propertyName))
            {
                subscriptions[propertyName] = new List<Func<object, Task>>();
            }

            subscriptions[propertyName].Add(async value => await func((T)value).ConfigureAwait(false));
        }

        #region Lifecycle Methods

        public virtual void OnInitialized() { }

        public virtual Task OnInitializedAsync()
        {
            return Task.CompletedTask;
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

        #endregion
    }
}
