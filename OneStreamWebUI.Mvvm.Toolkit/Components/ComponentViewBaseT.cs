using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public abstract class ComponentViewBase<TViewModel> : ComponentViewBase where TViewModel : ViewModelBase
    {
        private IViewModelParameterSetter? viewModelParameterSetter;

        protected internal TViewModel DataContext { get; set; } = null!;

        public ComponentViewBase() { }
        internal ComponentViewBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            SetDataContext();
        }

        #nullable disable
        private void SetDataContext()
        {
            DataContext ??= ServiceProvider?.GetRequiredService<TViewModel>();
            DataContext.ServiceProvider = ServiceProvider;
        }
        #nullable enable

        public TValue Bind<TValue>(Expression<Func<TViewModel, TValue>> property)
        {
            if (DataContext is null)
            {
                throw new InvalidOperationException($"{nameof(DataContext)} is not set");
            }
            return AddBinding(DataContext, property);
        }

        protected override void OnInitialized()
        {
            SetDataContext();
            base.OnInitialized();
            DataContext?.OnInitialized();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await DataContext!.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            SetParameters();
            base.OnParametersSet();
            DataContext?.OnParametersSet();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await DataContext.OnParametersSetAsync();
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);

            if (DataContext != null)
            {
                await DataContext.SetParametersAsync(parameters);
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            DataContext?.OnAfterRender(firstRender);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await DataContext!.OnAfterRenderAsync(firstRender);
        }

        protected override bool ShouldRender()
        {
            return DataContext?.ShouldRender() ?? true;
        }

        private void SetParameters()
        {
            if (DataContext is null)
            {
                throw new InvalidOperationException($"{nameof(DataContext)} is not set");
            }
            viewModelParameterSetter ??= ServiceProvider.GetRequiredService<IViewModelParameterSetter>();
            viewModelParameterSetter.ResolveAndSet(this, DataContext);
        }

    }
}
