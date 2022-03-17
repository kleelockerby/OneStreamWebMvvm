using System;
using System.Windows.Input;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class RelayCommand
    {
        private Action execute;
        private Action? canExecuteChanged;
        private bool canExecute;

        public Func<bool> TargetCanExecuteMethod;

        public bool CanExecute()
        {
            if (TargetCanExecuteMethod != null)
            {
                return TargetCanExecuteMethod();
            }
            if (execute != null)
            {
                return true;
            }
            return false;
        }

        public RelayCommand(Action Execute, Func<bool> canExecuteTarget, Action CanExecuteChanged)
        {
            this.execute = Execute;
            this.TargetCanExecuteMethod = canExecuteTarget;
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



/*public class RelayCommand
    {        
        private Action execute;
        private Action? canExecuteChanged;
        private Func<bool> canExecute;

        public RelayCommand(Action Execute, Func<bool> CanExecute)
        {
            this.execute = Execute;
            this.canExecute = CanExecute;
        }

        public RelayCommand(Action Execute, Func<bool> CanExecute, Action CanExecuteChanged)
        {
            this.execute = Execute;
            this.canExecute = CanExecute;
            this.canExecuteChanged = CanExecuteChanged;
        }

        public bool CanExecute()
        {
            if (canExecute != null)
            {
                return canExecute.Invoke();
            }
            return true;
        }

        public void Execute()
        {
            if (CanExecute() )
            {
                execute?.Invoke();
            }
            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            canExecuteChanged?.Invoke();
        }
    }*/