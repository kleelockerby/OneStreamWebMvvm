using Microsoft.AspNetCore.Components;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public interface IViewModelParameterSetter
    {
        void ResolveAndSet(ComponentBase component, ViewModelBase viewModel);
    }
}
