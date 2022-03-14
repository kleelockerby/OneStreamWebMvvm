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


/*

<button type="button" class="@($"btn {Class}")" @onclick="Callback" @attributes="InputAttributes">@ChildContent</button>

private void Callback()
{
    Command?.Execute(Parameter);
}

<CommandButton class="btn-sm btn-primary" Command="EditCommand" Parameter="Parameter">Edit</CommandButton>
<CommandButton class="btn-sm btn-danger" Command="DeleteCommand" Parameter="Parameter">Delete</CommandButton>

[Parameter] public ICommand EditCommand { get; set; }
[Parameter] public ICommand SaveCommand { get; set; }
[Parameter] public ICommand DeleteCommand { get; set; }
[Parameter] public object Parameter { get; set; }

SaveCommand = new DelegateCommand(param => this.Save(), param => this.CanSave);

private bool CanSave (object obj)
{
  if(condition) 
    return true;
  else
    return false;
}

private void Save()
{
    //ObjectService.Save()....
}
*/