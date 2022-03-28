using System;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class DelegateCommand
    {
        private Func<object, bool> canExecute;
        private Action<object> execute;
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> Execute) : this(Execute, null) { }

        public DelegateCommand(Action<object> Execute, Func<object, bool> CanExecute)
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

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}