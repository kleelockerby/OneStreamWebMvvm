
namespace OneStreamWebUI.Mvvm.Toolkit
{
    public abstract class WeakEventListenerBase<TItem, TArgs> : IWeakEventListener where TItem : class where TArgs : EventArgs
    {
        private readonly WeakReference<TItem> source;
        private readonly WeakReference<Action<TItem, TArgs>> handler;

        public WeakEventListenerBase(TItem Source, Action<TItem, TArgs> Handler)
        {
            this.source = new WeakReference<TItem>(Source ?? throw new ArgumentNullException(nameof(Source)));
            this.handler = new WeakReference<Action<TItem, TArgs>>(Handler ?? throw new ArgumentNullException(nameof(Handler)));
        }

        public bool IsAlive => handler.TryGetTarget(out _) && source.TryGetTarget(out _);

        protected void HandleEvent(object sender, TArgs e)
        {
            if (handler.TryGetTarget(out var handlerOut))
            {
                handlerOut((TItem)sender, e);
            }
            else
            {
                StopListening();
            }
        }

        public object? Source
        {
            get
            {
                if (source.TryGetTarget(out var sourceOut))
                {
                    return sourceOut;
                }
                return null;
            }
        }

        public Delegate? Handler
        {
            get
            {
                if (handler.TryGetTarget(out var handlerOut))
                {
                    return handlerOut;
                }
                return null;
            }
        }

        public void StopListening()
        {
            if (source.TryGetTarget(out var sourceOut))
            {
                StopListening(sourceOut);
            }
        }
        protected abstract void StopListening(TItem source);
    }
}