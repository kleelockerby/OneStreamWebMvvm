using System;
using System.Windows.Input;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class RelayCommand<TItem> : RelayCommand
    {
        private readonly Action<TItem>? action;
        private readonly Func<TItem, bool>? canExecute;

        protected RelayCommand() { }

        public RelayCommand(Action<TItem> action) : this(action, null!) { }

        public RelayCommand(Action<TItem> action, Func<TItem, bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }
            return canExecute((TItem)parameter);
        }
    }
}
