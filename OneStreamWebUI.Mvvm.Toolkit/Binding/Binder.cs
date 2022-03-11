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

            var propertyInfo = ResolveBindingContext(viewModel, propertyExpression);

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