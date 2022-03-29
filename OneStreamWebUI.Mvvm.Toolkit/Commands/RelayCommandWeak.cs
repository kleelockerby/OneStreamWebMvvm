using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using System.Reflection;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class RelayCommandWeak<TItem>
    {
        private readonly WeakAction<TItem>? execute;
        private readonly WeakFunc<TItem, bool>? canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommandWeak(Action<TItem> execute, bool keepTargetAlive = false) : this(execute, null!, keepTargetAlive) { }

        public RelayCommandWeak(Action<TItem> Execute, Func<TItem, bool> CanExecute, bool keepTargetAlive = false)
        {
            if (Execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this.execute = new WeakAction<TItem>(Execute);

            if (CanExecute != null)
            {
                canExecute = new WeakFunc<TItem, bool>(CanExecute, keepTargetAlive);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object? parameter)
        {
            if (canExecute == null)
            {
                return true;
            }

            if (canExecute.IsStatic || canExecute.IsAlive)
            {
                if (parameter == null && typeof(TItem).GetTypeInfo().IsValueType)
                {
                    return canExecute.Execute(default(TItem?));
                }

                if (parameter == null || parameter is TItem)
                {
                    return (canExecute.Execute((TItem?)parameter));
                }
            }

            return false;
        }

        public virtual void Execute(object parameter)
        {
            var val = parameter;
            if (parameter != null && parameter.GetType() != typeof(TItem))
            {
                if (parameter is IConvertible)
                {
                    val = Convert.ChangeType(parameter, typeof(TItem), null);
                }
            }

            if (CanExecute(val) && execute != null && (execute.IsStatic || execute.IsAlive))
            {
                if (val == null)
                {
                    if (typeof(TItem).GetTypeInfo().IsValueType)
                    {
                        execute.Execute(default(TItem?)!);
                    }
                    else
                    {
                        execute.Execute((TItem?)val!);
                    }
                }
                else
                {
                    execute.Execute((TItem?)val!);
                }
            }
        }
    }
}