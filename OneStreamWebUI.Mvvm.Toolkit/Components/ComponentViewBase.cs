#nullable enable
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class ComponentViewBase : ComponentBase
    {
        private IBindingFactory bindingFactory = null!;
        private HashSet<IBinding> bindings = new();
        private IWeakEventManager weakEventManager = null!;

        private int index = 1;

        [Inject] protected IServiceProvider ServiceProvider { get; set; } = default!;

        protected ComponentViewBase() { }
        protected internal ComponentViewBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            InitializeDependencies();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitializeDependencies();
        }

        private void InitializeDependencies()
        {
            bindingFactory = ServiceProvider.GetRequiredService<IBindingFactory>();
            weakEventManager = ServiceProvider.GetRequiredService<IWeakEventManager>();
        }

        public TValue Bind<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> property) where TViewModel : ViewModelBase
        {
            return AddBinding(viewModel, property);
        }

        public virtual TValue AddBinding<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> propertyExpression) where TViewModel : ViewModelBase
        {
            PropertyInfo? propertyInfo = ResolveBindingContext(viewModel, propertyExpression);

            IBinding? binding = bindingFactory.Create(viewModel, propertyInfo, weakEventManager);
            if (bindings.Contains(binding))
            {
                return (TValue)binding.GetValue();
            }

            weakEventManager.AddWeakEventListener<IBinding, EventArgs>(binding, nameof(Binding.BindingValueChanged), OnBindingValueChanged);
            binding.Initialize();
            bindings.Add(binding);

            return (TValue)binding.GetValue();
        }

        internal virtual void OnBindingValueChanged(object sender, EventArgs e)
        {
            Console.WriteLine($"StateHasChanged ViewComponentBase, Count:{index++}");
            InvokeAsync(StateHasChanged);
        }

        protected static PropertyInfo ResolveBindingContext<TViewModel, TValue>(TViewModel viewModel, Expression<Func<TViewModel, TValue>> property)
        {
            string propertyName = string.Empty;
            try
            {
                if ((viewModel != null) && (property != null))
                {
                    if (property.Body is MemberExpression m)
                    {
                        if (m.Member is PropertyInfo propertyInfo)
                        {
                            if (typeof(TViewModel).GetProperty(propertyInfo.Name) is not null)
                            {
                                propertyName = propertyInfo.Name;
                                return propertyInfo;
                            }
                        }
                    }
                    else
                    {
                        throw new BindingException($"Cannot find property {propertyName} in type {viewModel.GetType().FullName}");
                    }
                }
            }
            catch (BindingException)
            {
                throw new BindingException($"Cannot find property {propertyName} in type {viewModel.GetType().FullName}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An Unknow Exception Occured: {ex.Message}");
            }
            return null!;
        }
    }
}
