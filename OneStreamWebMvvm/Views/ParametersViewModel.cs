using Microsoft.AspNetCore.Components;
using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class ParametersViewModel : ViewModelBase
    {
        private NavigationManager? navigationManager;
        
        [Parameter] public string? ParamName { get; set; }
        
        public string? NewName { get; set; }
        
        public void NavigateToNewName()
        {
            if (string.IsNullOrEmpty(NewName))
            {
                return;
            }

            this.navigationManager?.NavigateTo($"/parametersview/{NewName}");
        }

        protected override void OnInitialized()
        {
            navigationManager = ServiceProvider.GetRequiredService<NavigationManager>();
        }
    }
}
