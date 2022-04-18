using System;
using System.Windows.Input;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class RelayCommandNew : ICommand
    {
        private Func<object, bool> canExecute;
        private Action<object> execute;
        public event EventHandler? CanExecuteChanged;

        public RelayCommandNew(Action<object> Execute) : this(Execute, null!) { }

        public RelayCommandNew(Action<object> Execute, Func<object, bool> CanExecute)
        {
            this.execute = Execute;
            this.canExecute = CanExecute;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }
            return canExecute(parameter);
        }

        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            var canExecuteChanged = CanExecuteChanged;
            if (canExecuteChanged != null)
            {
                canExecuteChanged(this, e);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }
    }
}