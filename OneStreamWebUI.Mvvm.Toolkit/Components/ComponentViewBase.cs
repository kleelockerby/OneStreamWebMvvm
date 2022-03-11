#nullable enable
using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class ComponentViewBase : ComponentBase, IDisposable
    {
        private AsyncServiceScope? scope;

        [Inject] IServiceScopeFactory ScopeFactory { get; set; } = default!;
        [Inject] protected IServiceProvider RootServiceProvider { get; set; } = default!;

        protected bool IsDisposed { get; private set; }

        public IBinder Binder { get; private set; } = null!;

        protected new IServiceProvider ScopedServices
        {
            get
            {
                if (ScopeFactory == null)
                {
                    throw new InvalidOperationException("Services cannot be accessed before the component is initialized.");
                }

                if (IsDisposed)
                {
                    throw new ObjectDisposedException(GetType().Name);
                }

                scope ??= ScopeFactory.CreateAsyncScope();
                return scope.Value.ServiceProvider;
            }
        }

        protected ComponentViewBase() { }

        protected internal ComponentViewBase(IServiceProvider services)
        {
            RootServiceProvider = services;
            ScopeFactory = services.GetRequiredService<IServiceScopeFactory>();
            InitializeDependencies();
        }

        private void InitializeDependencies()
        {
            Binder = ScopedServices.GetRequiredService<IBinder>();
            Binder.ValueChangedCallback = BindingOnBindingValueChanged;
        }

        protected internal TValue Bind<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> property) where TViewModel : ViewModelBase
        {
            return AddBinding(viewModel, property);
        }

        public virtual TValue AddBinding<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> propertyExpression) where TViewModel : ViewModelBase
        {
            return Binder.Bind(viewModel, propertyExpression);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitializeDependencies();
        }

        internal virtual void BindingOnBindingValueChanged(object sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                scope?.Dispose();
                scope = null;
                Dispose(true);
                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }

    }
}
