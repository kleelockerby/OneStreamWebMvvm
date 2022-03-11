using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.ComponentModel;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class WeakEventManager : IWeakEventManager
    {
        private readonly Dictionary<IWeakEventListener, Delegate> listeners = new();

        public void AddWeakEventListener<TItem, TArgs>(TItem source, string eventName, Action<TItem, TArgs> handler) where TItem : class where TArgs : EventArgs
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            listeners.Add(new WeakEventListener<TItem, TArgs>(source, eventName, handler), handler);
        }

        public void AddWeakEventListener<TViewModel>(TViewModel source, Action<TViewModel, PropertyChangedEventArgs> handler) where TViewModel : class, INotifyPropertyChanged
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            listeners.Add(new WeakPropertyChangedEventListener<TViewModel>(source, handler), handler);
        }

        public void AddWeakEventListener<TViewModel>(TViewModel source, Action<TViewModel, NotifyCollectionChangedEventArgs> handler) where TViewModel : class, INotifyCollectionChanged
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            listeners.Add(new WeakCollectionChangedEventListener<TViewModel>(source, handler), handler);
        }

        public void RemoveWeakEventListener<TViewModel>(TViewModel source) where TViewModel : class
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }

            var toRemove = new List<IWeakEventListener>();
            foreach (var listener in listeners.Keys)
            {
                if (!listener.IsAlive)
                {
                    toRemove.Add(listener);
                }
                else if (listener.Source == source)
                {
                    listener.StopListening();
                    toRemove.Add(listener);
                }
            }

            foreach (var item in toRemove)
            {
                listeners.Remove(item);
            }
        }

        public void ClearWeakEventListeners()
        {
            foreach (var listener in listeners.Keys)
            {
                if (listener.IsAlive)
                {
                    listener.StopListening();
                }
            }
            listeners.Clear();
        }
    }
}

