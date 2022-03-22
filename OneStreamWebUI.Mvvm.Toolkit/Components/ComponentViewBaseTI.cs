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

        protected internal TViewModel DataContext { get; set; } = null!;

        public ComponentViewBase() { }
        internal ComponentViewBase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            SetDataContext();
        }

        public TValue Bind<TValue>(Expression<Func<TViewModel, TValue>> property)
        {
            if (DataContext is null)
            {
                throw new InvalidOperationException($"{nameof(DataContext)} is not set");
            }
            return AddBinding(DataContext, property);
        }

        private void SetDataContext()
        {
            DataContext ??= ServiceProvider?.GetRequiredService<TViewModel>();
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

        protected override bool ShouldRender()
        {
            return DataContext?.ShouldRender() ?? true;
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

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);

            if (DataContext != null)
            {
                await DataContext.SetParametersAsync(parameters);
            }
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
