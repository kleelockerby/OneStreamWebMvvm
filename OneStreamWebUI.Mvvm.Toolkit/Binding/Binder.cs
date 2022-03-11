using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Linq.Expressions;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public interface IBinder
    {
        Action<IBinding, EventArgs>? ValueChangedCallback { get; set; }
        TValue Bind<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> propertyExpression) where TViewModel : ViewModelBase;
    }

    internal class Binder : IBinder, IDisposable
    {
        private readonly IBindingFactory bindingFactory;
        private readonly HashSet<IBinding> bindings = new();
        private readonly IWeakEventManager weakEventManager;
        private bool isDisposed;
        
        public Action<IBinding, EventArgs>? ValueChangedCallback { get; set; }
        
        public Binder(IBindingFactory BindingFactory, IWeakEventManager WeakEventManager)
        {
            this.bindingFactory = BindingFactory;
            this.weakEventManager = WeakEventManager;
        }

        public TValue Bind<TViewModel, TValue>( TViewModel viewModel, Expression<Func<TViewModel, TValue>> propertyExpression) where TViewModel : ViewModelBase
        {
            ThrowIfDisposed();

            if (ValueChangedCallback is null)
            {
                throw new BindingException($"{nameof(ValueChangedCallback)} is null");
            }

            var propertyInfo = ValidateAndResolveBindingContext(viewModel, propertyExpression);

            var binding = bindingFactory.Create(viewModel, propertyInfo, weakEventManager);
            if (bindings.Contains(binding))
            {
                return (TValue)binding.GetValue();
            }

            weakEventManager.AddWeakEventListener(binding, nameof(Binding.BindingValueChanged), ValueChangedCallback);
            binding.Initialize();

            bindings.Add(binding);

            return (TValue)binding.GetValue();
        }

        protected static PropertyInfo ValidateAndResolveBindingContext<TViewModel, TValue>( TViewModel viewModel, Expression<Func<TViewModel, TValue>> property) where TViewModel : ViewModelBase
        {
            if (viewModel is null)
            {
                throw new BindingException("ViewModelType is null");
            }

            if (property is null)
            {
                throw new BindingException("Property expression is null");
            }

            if (property.Body is not MemberExpression { Member: PropertyInfo p })
            {
                throw new BindingException("Binding member needs to be a property");
            }

            if (typeof(TViewModel).GetProperty(p.Name) is null)
            {
                throw new BindingException($"Cannot find property {p.Name} in type {viewModel.GetType().FullName}");
            }

            return p;
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
                ThrowIfDisposed();
                isDisposed = true;
                DisposeBindings();
            }
        }

        private void DisposeBindings()
        {
            foreach (var binding in bindings)
            {
                weakEventManager.RemoveWeakEventListener(binding);
                binding.Dispose();
            }
        }

        private void ThrowIfDisposed()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(Binder));
            }
        }

        ~Binder()
        {
            Dispose(false);
        }

        #endregion
    }
}