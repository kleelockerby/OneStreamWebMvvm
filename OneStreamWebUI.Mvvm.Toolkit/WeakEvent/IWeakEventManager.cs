using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.ComponentModel;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    internal interface IWeakEventManager
    {
        void AddWeakEventListener<TItem, TArgs>(TItem source, string eventName, Action<TItem, TArgs> handler) where TItem : class where TArgs : EventArgs;
        void AddWeakEventListener<TViewModel>(TViewModel source, Action<TViewModel, PropertyChangedEventArgs> handler) where TViewModel : class, INotifyPropertyChanged;
        void AddWeakEventListener<TViewModel>(TViewModel source, Action<TViewModel, NotifyCollectionChangedEventArgs> handler) where TViewModel : class, INotifyCollectionChanged;
        void RemoveWeakEventListener<TViewModel>(TViewModel source) where TViewModel : class;
        void ClearWeakEventListeners();
    }
}
