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
    internal class ComponentViewBase<TViewModel> : ComponentViewBase where TViewModel : ViewModelBase
    {
        private IViewModelParameterSetter? viewModelParameterSetter;

        protected ComponentViewBase() { }

        protected internal ComponentViewBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            SetBindingContext();
        }

        protected TViewModel BindingContext { get; set; }

        private void SetBindingContext()
        {
            BindingContext ??= ScopedServices.GetRequiredService<TViewModel>();
            BindingContext.RootServiceProvider = RootServiceProvider;
        }

        private void SetParameters()
        {
            if (IsDisposed)
                return;

            if (BindingContext is null)
                throw new InvalidOperationException($"{ nameof(BindingContext) } is not set");

            viewModelParameterSetter ??= ScopedServices.GetRequiredService<IViewModelParameterSetter>();
            viewModelParameterSetter.ResolveAndSet(this, BindingContext);
        }

        protected internal TValue Bind<TValue>(Expression<Func<TViewModel, TValue>> property)
        {
            if (BindingContext is null)
                throw new InvalidOperationException($"{ nameof(BindingContext) } is not set");

            return AddBinding(BindingContext, property);
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
            return BindingContext!.ShouldRender();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            BindingContext!.OnAfterRender(firstRender);
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
