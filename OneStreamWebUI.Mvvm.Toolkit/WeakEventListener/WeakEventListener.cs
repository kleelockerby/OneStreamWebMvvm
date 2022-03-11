using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class WeakEventListener<TItem, TArgs> : WeakEventListenerBase<TItem, TArgs> where TItem : class where TArgs : EventArgs
    {
        private readonly EventInfo eventInfo;

        public WeakEventListener(TItem source, string eventName, Action<TItem, TArgs> handler) : base(source, handler)
        {
            eventInfo = source.GetType().GetEvent(eventName) ?? throw new ArgumentException("Unknown Event Name", nameof(eventName));
            if (eventInfo.EventHandlerType == typeof(EventHandler<TArgs>))
            {
                eventInfo.AddEventHandler(source, new EventHandler<TArgs>(HandleEvent!));
            }
            else
            {
                eventInfo.AddEventHandler(source, Delegate.CreateDelegate(eventInfo.EventHandlerType!, this, nameof(HandleEvent)));
            }
        }

        protected override void StopListening(TItem source)
        {
            if (eventInfo.EventHandlerType == typeof(EventHandler<TArgs>))
            {
                eventInfo.RemoveEventHandler(source, new EventHandler<TArgs>(HandleEvent!));
            }
            else
            {
                eventInfo.RemoveEventHandler(source, Delegate.CreateDelegate(eventInfo.EventHandlerType!, this, nameof(HandleEvent)));
            }
        }
    }
}
