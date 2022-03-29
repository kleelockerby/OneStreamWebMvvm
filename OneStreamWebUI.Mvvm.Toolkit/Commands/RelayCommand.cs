using System;
using System.Windows.Input;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class RelayCommand
    {
        private Func<bool>? canExecute;
        private Action? execute;
        public event EventHandler? CanExecuteChanged;

        protected RelayCommand() { }

        public RelayCommand(Action Execute)
        {
            this.execute = Execute;
        }

        public RelayCommand(Action Execute, Func<bool> CanExecute) : this(Execute)
        {
            this.canExecute = CanExecute;
        }

        public void Execute()
        {
            execute?.Invoke();
        }

        public virtual bool CanExecute()
        {
            if (canExecute == null)
            {
                return true;
            }
            return canExecute();
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