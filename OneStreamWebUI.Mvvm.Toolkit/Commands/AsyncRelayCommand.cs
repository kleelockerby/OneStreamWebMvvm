using System;
using System.Windows.Input;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class AsyncRelayCommand
    {
        private Func<Task, bool>? canExecuteAsync;
        private Func<bool>? canExecute;
        private Func<Task>? executeAsync;
        public event EventHandler? CanExecuteChanged;

        protected AsyncRelayCommand() { }

        public AsyncRelayCommand(Func<Task> Execute)
        {
            this.executeAsync = Execute;
        }

        public AsyncRelayCommand(Func<Task> ExecuteAsync, Func<bool> CanExecute, Func<Task, bool>? CanExecuteAsync = null) : this(ExecuteAsync)
        {           
            this.canExecute = CanExecute;
            this.canExecuteAsync = CanExecuteAsync;
        }

        public async void Execute()
        {
            await executeAsync?.Invoke()!;
        }

        public async Task ExecuteAsync()
        {
            await executeAsync?.Invoke()!;
        }

        public virtual bool CanExecute()
        {
            if (canExecute == null)
            {
                return true;
            }
            return canExecute();
        }











        /*private readonly Func<Task>? execute;
        private readonly Func<bool>? canExecute;
        private Task? task;

        public AsyncRelayCommand(Func<Task> Execute, Func<bool> CanExecute = null)
        {
            if (Execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
            this.execute = Execute;
            this.canExecute = CanExecute;
        }





        //public event EventHandler Executed;

        public bool CanExecute(object parameter)
        {
            return this.canExecute is null || this.canExecute(parameter);
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
        }*/
    }
}
