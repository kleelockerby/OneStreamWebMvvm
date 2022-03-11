using System;
using System.ComponentModel;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class WeakPropertyChangedEventListener<TViewModel> : WeakEventListenerBase<TViewModel, PropertyChangedEventArgs> where TViewModel : class, INotifyPropertyChanged
    {
        public WeakPropertyChangedEventListener(TViewModel source, Action<TViewModel, PropertyChangedEventArgs> handler) : base(source, handler)
        {
            source.PropertyChanged += HandleEvent;
        }

        protected override void StopListening(TViewModel source)
        {
            source.PropertyChanged -= HandleEvent;
        }

    }
}
