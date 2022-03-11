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
    public class ComponentViewBase : ComponentBase, IDisposable, IAsyncDisposable
    {
        //private IWeakEventManagerFactory weakEventManagerFactory = null;
        private IWeakEventManager weakEventManager = null;
        private IBindingFactory bindingFactory = null!;
        private HashSet<IBinding> bindings = new();

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
            //weakEventManagerFactory = ServiceProvider.GetRequiredService<IWeakEventManagerFactory>();
            weakEventManager = ServiceProvider.GetRequiredService<IWeakEventManager>();
            bindingFactory = ServiceProvider.GetRequiredService<IBindingFactory>();
            //weakEventManager = weakEventManagerFactory.Create();
        }

        public TValue Bind<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> property) where TViewModel : ViewModelBase
        {
            return AddBinding(viewModel, property);
        }

        public virtual TValue AddBinding<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> propertyExpression) where TViewModel : ViewModelBase
        {
            var propertyInfo = ValidateAndResolveBindingContext(viewModel, propertyExpression);

            //var binding = bindingFactory.Create(viewModel, propertyInfo, weakEventManagerFactory.Create());
            var binding = bindingFactory.Create(viewModel, propertyInfo, weakEventManager);

            if (bindings.Contains(binding))
            {
                return (TValue)binding.GetValue();
            }

            weakEventManager.AddWeakEventListener<IBinding, EventArgs>(binding, nameof(Binding.BindingValueChanged), OnBindingValueChanged);
            binding.Initialize();
            bindings.Add(binding);

            return (TValue)binding.GetValue();
        }

        internal virtual void OnBindingValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine($"StateHasChanged ViewComponentBase, Count:{index++}");
            InvokeAsync(StateHasChanged);
        }

        protected static PropertyInfo ValidateAndResolveBindingContext<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> property)
        {
            if (viewModel is null)
                throw new BindingException("ViewModelType is null");

            if (property is null)
                throw new BindingException("Property expression is null");

            if (!(property.Body is MemberExpression m))
                throw new BindingException("Binding member needs to be a property");

            if (!(m.Member is PropertyInfo p))
                throw new BindingException("Binding member needs to be a property");

            if (typeof(TViewModel).GetProperty(p.Name) is null)
                throw new BindingException($"Cannot find property {p.Name} in type {viewModel.GetType().FullName}");

            return p;
        }

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (bindings is not null)
                {
                    DisposeBindings();
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();

            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual ValueTask DisposeAsyncCore()
        {
            if (bindings is not null)
            {
                DisposeBindings();
            }
            return default;
        }

        private void DisposeBindings()
        {
            foreach (var binding in bindings)
            {
                weakEventManager.RemoveWeakEventListener(binding);
                binding.Dispose();
            }
        }

        ~ComponentViewBase()
        {
            Dispose(false);
        }

        #endregion
    }

}
