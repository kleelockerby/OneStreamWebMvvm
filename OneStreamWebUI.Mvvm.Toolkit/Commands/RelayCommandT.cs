using System;
using System.Windows.Input;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class RelayCommand<T>
    {
        private Action<T> execute;
        private Func<T, bool> canExecute;
        private Action<T> canExecuteChanged;

        public event EventHandler Executed;

        public RelayCommand(Action<T> Execute, Func<T, bool> CanExecute)
        {
            this.execute = Execute;
            this.canExecute = CanExecute;
        }

        public RelayCommand(Action<T> Execute, Func<T, bool> CanExecute, Action<T> CanExecuteChanged)
        {
            this.execute = Execute;
            this.canExecute = CanExecute;
            this.canExecuteChanged = CanExecuteChanged;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute is null || this.canExecute((T)parameter);
        }
        
        public void Execute(object parameter)
        {
            this.InvokeAction(parameter);
            OnExecuted();
        }

        public void InvokeAction(object parameter)
        {
            this.execute((T)parameter);
        }

        protected void OnExecuted()
        {
            this.Executed?.Invoke(this, null);
        }
    }
}
