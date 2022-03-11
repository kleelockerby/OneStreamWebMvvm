using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public interface IMessageAggregator
    {
        void Subscribe(object subscriber);
        void Unsubscribe(object subscriber);
        Task PublishAsync(object message);
    }
}
