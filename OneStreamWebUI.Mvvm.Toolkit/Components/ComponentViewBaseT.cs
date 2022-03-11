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
            base.OnInitialized();
            SetBindingContext();
            SetParameters();
            BindingContext?.OnInitialized();
        }

        protected override Task OnInitializedAsync()
        {
            return BindingContext?.OnInitializedAsync() ?? Task.CompletedTask;
        }

        private void SetBindingContext()
        {
            BindingContext ??= ServiceProvider.GetRequiredService<TViewModel>();
        }

        public TValue Bind<TValue>(Expression<Func<TViewModel, TValue>> property)
        {
            if (BindingContext is null)
            {
                throw new InvalidOperationException($"{nameof(BindingContext)} is not set");
            }
            return AddBinding(BindingContext, property);
        }

        private void SetParameters()
        {
            if (BindingContext is null)
            {
                throw new InvalidOperationException($"{nameof(BindingContext)} is not set");
            }
            viewModelParameterSetter ??= ServiceProvider.GetRequiredService<IViewModelParameterSetter>();
            viewModelParameterSetter.ResolveAndSet(this, BindingContext);
        }

        protected override void OnParametersSet()
        {
            SetParameters();
            BindingContext?.OnParametersSet();
        }

        protected override Task OnParametersSetAsync()
        {
            return BindingContext?.OnParametersSetAsync() ?? Task.CompletedTask;
        }

        protected override bool ShouldRender()
        {
            return BindingContext?.ShouldRender() ?? true;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            BindingContext?.OnAfterRender(firstRender);
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            return BindingContext?.OnAfterRenderAsync(firstRender) ?? Task.CompletedTask;
        }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters).ConfigureAwait(false);
            if (BindingContext != null)
            {
                await BindingContext.SetParametersAsync(parameters).ConfigureAwait(false);
            }
        }
    }
}
