using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public interface IHandleViewModelMessage<TMessage>
    {
        Task HandleAsync(TMessage message);
    }
}
