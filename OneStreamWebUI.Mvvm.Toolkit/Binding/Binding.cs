using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    internal class Binding : IBinding
    {
        private readonly IWeakEventManager weakEventManager;
        private INotifyCollectionChanged? boundCollection;
        private bool isCollection;

        public INotifyPropertyChanged Source { get; }
        public PropertyInfo PropertyInfo { get; }

        public event EventHandler? BindingValueChanged;
        
        public Binding(INotifyPropertyChanged isource, PropertyInfo propertyInfo, IWeakEventManager iweakEventManager)
        {
            weakEventManager = iweakEventManager ?? throw new ArgumentNullException(nameof(iweakEventManager));
            Source = isource ?? throw new ArgumentNullException(nameof(isource));
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }

        public void Initialize()
        {
            isCollection = typeof(INotifyCollectionChanged).IsAssignableFrom(PropertyInfo.PropertyType);
            AddCollectionBindings();
            weakEventManager.AddWeakEventListener(Source, SourceOnPropertyChanged);
        }

        public object GetValue()
        {
            var propInfo = PropertyInfo.GetValue(Source, null)!;
            return propInfo;
        }

        private void SourceOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is null)
            {
                BindingValueChanged?.Invoke(this, EventArgs.Empty);
                return;
            }

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

        private void AddCollectionBindings()
        {
            if (!isCollection || GetValue() is not INotifyCollectionChanged collection)
            {
                return;
            }

            weakEventManager.AddWeakEventListener(collection, OnCollectionChanged);
            boundCollection = collection;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            BindingValueChanged?.Invoke(this, EventArgs.Empty);
        }

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                weakEventManager.RemoveWeakEventListener(Source);
            }
        }
    }
}
