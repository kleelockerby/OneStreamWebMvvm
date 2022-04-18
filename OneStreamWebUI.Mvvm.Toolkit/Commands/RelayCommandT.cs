using System;
using System.Windows.Input;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class RelayCommand<T>
    {
        private Action<T> execute;
        private Func<T, bool> canExecute;
        public event EventHandler? CanExecuteChanged;

        public event EventHandler Executed;

        protected RelayCommand() { }

        public RelayCommand(Action<T> Execute) : this(Execute, null!)
        {
            this.execute = Execute;
        }

        public RelayCommand(Action<T> Execute, Func<T, bool> CanExecute)
        {
            this.execute = Execute;
            this.canExecute = CanExecute;
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

        public bool CanExecute(T? parameter)
        {
            return this.canExecute?.Invoke(parameter!) != false;
        }

        public bool CanExecute(object? parameter)
        {
            if (default(T) is not null &&
                parameter is null)
            {
                return false;
            }

            return CanExecute((T?)parameter);
        }

        public void Execute(T? parameter)
        {
            if (CanExecute(parameter))
            {
                this.execute(parameter);
            }
        }

        public void Execute(object parameter)
        {
            Execute((T?)parameter);
        }
    }
}
