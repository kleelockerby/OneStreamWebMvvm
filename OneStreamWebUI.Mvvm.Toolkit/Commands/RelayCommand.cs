using System;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class RelayCommand
    {
        private bool canExecute;
        private Action execute;
        private Action canExecuteChanged;

        public bool CanExecute
        {
            get { return canExecute; }
            set
            {
                if (canExecute != value)
                {
                    canExecute = value;
                    canExecuteChanged?.Invoke();
                }
            }
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
            execute?.Invoke();
        }
    }
}
