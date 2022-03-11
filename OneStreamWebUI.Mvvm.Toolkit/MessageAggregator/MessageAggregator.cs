using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class MessageAggregator : IMessageAggregator
    {
        private readonly List<Handler> handlers = new List<Handler>();

        public virtual void Subscribe(object subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException("subscriber");
            }

            lock (handlers)
            {
                if (handlers.Any(x => x.Matches(subscriber)))
                {
                    return;
                }

                handlers.Add(new Handler(subscriber));
            }
        }

        public async Task PublishAsync(object message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            Handler[] handlersToNotify;
            lock (handlers)
            {
                handlersToNotify = handlers.ToArray();
            }

            var messageType = message.GetType();
            var tasks = handlersToNotify.Select(h => h.Handle(messageType, message));
            await Task.WhenAll(tasks);

            var deadHandlers = handlersToNotify.Where(h => h.IsDead).ToList();
            if (deadHandlers.Any())
            {
                lock (handlers)
                {
                    foreach (var item in deadHandlers)
                    {
                        handlers.Remove(item);
                    }
                }
            }
        }

        public virtual void Unsubscribe(object subscriber)
        {
            if (subscriber == null)
            {
                throw new ArgumentNullException("subscriber");
            }

            lock (handlers)
            {
                var found = handlers.FirstOrDefault(x => x.Matches(subscriber));

                if (found != null)
                {
                    handlers.Remove(found);
                }
            }
        }

        private class Handler
        {
            private readonly WeakReference reference;
            private readonly Dictionary<Type, MethodInfo> supportedHandlers = new Dictionary<Type, MethodInfo>();

            public bool IsDead => reference.Target == null;

            public Handler(object handler)
            {
                reference = new WeakReference(handler);
                var interfaces = handler.GetType().GetTypeInfo().ImplementedInterfaces.Where(x => x.GetTypeInfo().IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandleViewModelMessage<>));

                foreach (var handleInterface in interfaces)
                {
                    var type = handleInterface.GetTypeInfo().GenericTypeArguments[0];
                    var method = handleInterface.GetRuntimeMethod("HandleAsync", new[] { type });

                    if (method != null)
                    {
                        supportedHandlers[type] = method;
                    }
                }
            }

            public bool Matches(object instance)
            {
                return reference.Target == instance;
            }

            public Task Handle(Type messageType, object message)
            {
                var target = reference.Target;
                if (target == null)
                {
                    return Task.FromResult(false);
                }

                var tasks = supportedHandlers
                    .Where(handler => handler.Key.GetTypeInfo().IsAssignableFrom(messageType.GetTypeInfo()))
                    .Select(pair => pair.Value.Invoke(target, new[] { message }))
                    .Select(result => (Task)result)
                    .ToList();

                return Task.WhenAll(tasks);
            }

            public bool Handles(Type messageType)
            {
                return supportedHandlers.Any(pair => pair.Key.IsAssignableFrom(messageType));
            }
        }
    }
}
