using System;
using System.Collections.Specialized;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class WeakCollectionChangedEventListener<TViewModel> : WeakEventListenerBase<TViewModel, NotifyCollectionChangedEventArgs> where TViewModel : class, INotifyCollectionChanged
    {
        public WeakCollectionChangedEventListener(TViewModel source, Action<TViewModel, NotifyCollectionChangedEventArgs> handler) : base(source, handler)
        {
            source.CollectionChanged += HandleEvent;
        }

        protected override void StopListening(TViewModel source)
        {
            source.CollectionChanged -= HandleEvent;
        }
    }
}
