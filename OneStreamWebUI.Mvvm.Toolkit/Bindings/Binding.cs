using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public interface IBinding : IDisposable
    {
        INotifyPropertyChanged Source { get; }
        PropertyInfo PropertyInfo { get; }
        event EventHandler? BindingValueChanged;
        void Initialize();
        object GetValue();
    }

    internal class Binding : IBinding
    {
        private readonly IWeakEventManager weakEventManager;
        private INotifyCollectionChanged? boundCollection;
        private bool isCollection;

        public Binding(INotifyPropertyChanged source, PropertyInfo propertyInfo, IWeakEventManager weakEventManager)
        {
            weakEventManager = weakEventManager;
            Source = source;
            PropertyInfo = propertyInfo;
        }

        public INotifyPropertyChanged Source { get; }
        public PropertyInfo PropertyInfo { get; }

        public event EventHandler? BindingValueChanged;

        public void Initialize()
        {
            isCollection = typeof(INotifyCollectionChanged).IsAssignableFrom(PropertyInfo.PropertyType);
            weakEventManager.AddWeakEventListener(Source, SourceOnPropertyChanged);
            AddCollectionBindings();
        }

        public object GetValue()
        {
            return PropertyInfo.GetValue(Source, null)!;
        }

        private void AddCollectionBindings()
        {
            if (!isCollection || GetValue() is not INotifyCollectionChanged collection)
            {
                return;
            }

            weakEventManager.AddWeakEventListener(collection, CollectionOnCollectionChanged);
            boundCollection = collection;
        }

        private void SourceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is null)
            {
                BindingValueChanged?.Invoke(this, EventArgs.Empty);
                return;
            }

            // This should just listen to the bindings property
            if (e.PropertyName != PropertyInfo.Name)
            {
                return;
            }

            if (isCollection)
            {
                // If our binding is a collection binding we need to remove the event
                // and reinitialize the collection bindings
                if (boundCollection != null)
                {
                    weakEventManager.RemoveWeakEventListener(boundCollection);
                }

                AddCollectionBindings();
            }


            BindingValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BindingValueChanged?.Invoke(this, EventArgs.Empty);
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
                if (boundCollection != null)
                {
                    weakEventManager.RemoveWeakEventListener(boundCollection);
                }

                weakEventManager.RemoveWeakEventListener(Source);
            }
        }

        #endregion

        #region Base overrides

        public override string ToString()
        {
            return $"{PropertyInfo?.DeclaringType?.Name}.{PropertyInfo?.Name}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Binding b && ReferenceEquals(b.Source, Source) && b.PropertyInfo.Name == PropertyInfo.Name;
        }

        public override int GetHashCode()
        {
            var hash = 13;
            hash = hash * 7 + Source.GetHashCode();
            hash = hash * 7 + PropertyInfo.Name.GetHashCode(StringComparison.InvariantCulture);

            return hash;
        }

        #endregion
    }
}