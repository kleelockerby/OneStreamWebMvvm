using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class ComponentViewBase<TViewModel, TItem> : ComponentViewBase where TViewModel : ViewModelCollectionBase<TItem> where TItem : class
    {
        private IViewModelParameterSetter? viewModelParameterSetter;

        protected internal TViewModel BindingContext { get; set; } = null!;

        public ComponentViewBase() { }
        internal ComponentViewBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            SetBindingContext();
        }

        public TValue Bind<TValue>(Expression<Func<TViewModel, TValue>> property)
        {
            if (BindingContext is null)
            {
                throw new InvalidOperationException($"{nameof(BindingContext)} is not set");
            }
            return AddBinding(BindingContext, property);
        }

        private void SetBindingContext()
        {
            BindingContext ??= ServiceProvider?.GetRequiredService<TViewModel>();
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

        private void SetParameters()
        {
            if (BindingContext is null)
            {
                throw new InvalidOperationException($"{nameof(BindingContext)} is not set");
            }
            viewModelParameterSetter ??= ServiceProvider.GetRequiredService<IViewModelParameterSetter>();
            viewModelParameterSetter.ResolveAndSet(this, BindingContext);
        }
    }
}
