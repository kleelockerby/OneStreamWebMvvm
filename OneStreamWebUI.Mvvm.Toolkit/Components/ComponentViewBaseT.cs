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

        protected internal TViewModel BindingContext { get; set; } = null!;

        public ComponentViewBase() { }
        internal ComponentViewBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            SetBindingContext();
        }

        protected override void OnInitialized()
        {
            SetBindingContext();
            base.OnInitialized();
            BindingContext?.OnInitialized();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await BindingContext!.OnInitializedAsync();
        }
#nullable disable
        private void SetBindingContext()
        {
            BindingContext ??= ServiceProvider?.GetRequiredService<TViewModel>();
            BindingContext.ServiceProvider = ServiceProvider;
        }
#nullable enable
        private void SetParameters()
        {
            if (BindingContext is null)
            {
                throw new InvalidOperationException($"{nameof(BindingContext)} is not set");
            }
            viewModelParameterSetter ??= ServiceProvider.GetRequiredService<IViewModelParameterSetter>();
            viewModelParameterSetter.ResolveAndSet(this, BindingContext);
        }

        public TValue Bind<TValue>(Expression<Func<TViewModel, TValue>> property)
        {
            if (BindingContext is null)
            {
                throw new InvalidOperationException($"{nameof(BindingContext)} is not set");
            }
            return AddBinding(BindingContext, property);
        }

        protected override void OnParametersSet()
        {
            SetParameters();
            base.OnParametersSet();
            BindingContext?.OnParametersSet();
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await BindingContext.OnParametersSetAsync();
        }

        protected override bool ShouldRender()
        {
            return BindingContext?.ShouldRender() ?? true;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            BindingContext?.OnAfterRender(firstRender);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            await BindingContext!.OnAfterRenderAsync(firstRender);
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);

            if (BindingContext != null)
            {
                await BindingContext.SetParametersAsync(parameters);
            }
        }
    }
}
