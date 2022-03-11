#nullable enable
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class ComponentViewBase : ComponentBase
    {
        private IBinder binder = null!;
        protected bool IsDisposed { get; private set; }
        private int index = 1;

        public bool IsInitialized { get; set; }

        [Inject] protected IServiceProvider ServiceProvider { get; set; } = null!;

        protected ComponentViewBase() { }
        protected internal ComponentViewBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            InitializeDependencies();
        }

        protected override void OnInitialized()
        {
            this.IsInitialized = true;
            InitializeDependencies();
        }

        private void InitializeDependencies()
        {
            binder = ServiceProvider.GetRequiredService<IBinder>();
            binder.ValueChangedCallback = OnBindingValueChanged;
        }

        public TValue Bind<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> property) where TViewModel : ViewModelBase
        {
            return AddBinding(viewModel, property);
        }

        public virtual TValue AddBinding<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> propertyExpression) where TViewModel : ViewModelBase
        {
            return binder.Bind(viewModel, propertyExpression);
        }

        internal virtual void OnBindingValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine($"StateHasChanged ViewComponentBase, Count:{index++}");
            InvokeAsync(StateHasChanged);
        }

        protected static PropertyInfo ResolveBindingContext<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> property)
        {
            string propertyName = string.Empty;
            try
            {
                if ((viewModel != null) && (property != null))
                {
                    if (property.Body is MemberExpression m)
                    {
                        if (m.Member is PropertyInfo propertyInfo)
                        {
                            if (typeof(TViewModel).GetProperty(propertyInfo.Name) is not null)
                            {
                                propertyName = propertyInfo.Name;
                                return propertyInfo;
                            }
                        }
                    }
                    else
                    {
                        throw new BindingException($"Cannot find property {propertyName} in type {viewModel.GetType().FullName}");
                    }
                }
            }
            catch (BindingException)
            {
                throw new BindingException($"Cannot find property {propertyName} in type {viewModel.GetType().FullName}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An Unknow Exception Occured: {ex.Message}");
            }
            return null!;
        }

    }
}
