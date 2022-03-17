using System;
using System.Windows.Input;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class RelayCommand
    {
        private Action execute;
        private Action? canExecuteChanged;
        private bool canExecute;

        public Func<bool> CanExecuteCommand;

        public bool CanExecuteCommandValue()
        {
            if (CanExecuteCommand != null)
            {
                return CanExecuteCommand();
            }
            return false;
        }

        public bool CanExecute
        {
            get { return canExecute; }
            set
            {
                if (canExecute != value)
                {
                    canExecute = value;
                    RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand(Action Execute, Func<bool> canExecuteTarget, Action CanExecuteChanged)
        {
            this.execute = Execute;
            this.CanExecuteCommand = canExecuteTarget;
            this.canExecuteChanged = CanExecuteChanged;
        }

        public RelayCommand(Action Execute, bool CanExecute)
        {
            this.execute = Execute;
            this.canExecute = CanExecute;
        }

        public RelayCommand(Action Execute, bool CanExecute, Action CanExecuteChanged)
        {
            this.execute = Execute;
            this.canExecute = CanExecute;
            this.canExecuteChanged = CanExecuteChanged;
        }

        public void Execute()
        {
            if (execute != null)
            {
                execute?.Invoke();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            canExecuteChanged?.Invoke();
        }
    }
}