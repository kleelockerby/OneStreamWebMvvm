#nullable enable
using System;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class ComponentViewBase : ComponentBase, IDisposable, IAsyncDisposable
    {
        private IBindingFactory bindingFactory = null!;
        private HashSet<IBinding> bindings = new();
        private IWeakEventManager weakEventManager = null!;
        private IViewModelParameterSetter? viewModelParameterSetter;
        private int index = 1;

        [Inject] protected IServiceProvider ServiceProvider { get; set; } = default!;

        protected ComponentViewBase() { }
        protected internal ComponentViewBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            InitializeDependencies();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitializeDependencies();
        }

        private void InitializeDependencies()
        {
            bindingFactory = ServiceProvider.GetRequiredService<IBindingFactory>();
            weakEventManager = ServiceProvider.GetRequiredService<IWeakEventManager>();
        }

        public TValue Bind<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> property) where TViewModel : ViewModelBase
        {
            return AddBinding(viewModel, property);
        }

        public virtual TValue AddBinding<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> propertyExpression) where TViewModel : ViewModelBase
        {
            PropertyInfo? propertyInfo = ResolveDataContext(viewModel, propertyExpression);

            IBinding? binding = bindingFactory.Create(viewModel, propertyInfo, weakEventManager);
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

        protected static PropertyInfo ResolveDataContext<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> property)
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

        /*protected static PropertyInfo ResolveDataContext<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> property)
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
        }*/

        protected void SetViewModelParameters(ViewModelBase viewModel)
        {
            viewModelParameterSetter ??= ServiceProvider.GetRequiredService<IViewModelParameterSetter>();
            viewModelParameterSetter.ResolveAndSet(this, viewModel);
        }
        #region IDisposable

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
                    bindings = null!;
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
                bindings = null!;
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
