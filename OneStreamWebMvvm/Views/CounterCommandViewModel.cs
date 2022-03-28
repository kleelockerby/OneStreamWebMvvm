using OneStreamWebUI.Mvvm.Toolkit;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Windows.Input;

namespace OneStreamWebMvvm
{
    public class CounterCommandViewModel : ViewModelBase
    {
        public RelayCommand UpdateCommand;
        public RelayCommand ResetCommand;

        public int MaxCount = 3;

        private int currentCount = 1;
        public int CurrentCount
        {
            get => currentCount;
            set
            {
                SetProperty(ref currentCount, value, nameof(CurrentCount));
            }
        }

        public string ClassName { get; set; } = "btn btn-primary";

        public CounterCommandViewModel()
        {        


            this.UpdateCommand = new RelayCommand(IncrementCount, CanIncrement);
            this.ResetCommand = new RelayCommand(ResetCounter, CanReset);
        }

        public void IncrementCount() => ++this.CurrentCount;
        public bool CanIncrement() => this.currentCount < this.MaxCount;

        public void ResetCounter() => this.CurrentCount = 1;
        public bool CanReset() => this.currentCount == this.MaxCount;
    }
}







/*


          
        public bool CanIncrement()
        {
            if (this.currentCount < this.maxCount)
            {
                this.IsActive = true;
                return true;
            }
            else
            {
                this.IsActive = false;
                return false;
            }      
        }

        public void Reset()
        {
            this.currentCount = 1;
            this.isActive = !this.isActive;
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static void RaiseCanExecuteChanged(RelayCommand command)
        {
            command.RaiseCanExecuteChanged();
        }
        
        
        
        public event EventHandler<CurrentPageChangedEventArgs> CurrentPageChanged;  See SMT PagingController
        
        public bool IsActive { get => isActive; set => SetProperty(ref isActive, value, nameof(IsActive)); }

         private bool _isEnabled = true;
         public bool IsEnabled
         {
             get => _isEnabled;
             set => EquatableValueChangeProcess(ref _isEnabled, value);
         }

         protected bool IsActive => UpdateCommand.CanExecute ?? IsActive ?? true;
        
         protected bool IsEnabled => DataContext?.Command?.CanExecute ?? DataContext?.IsEnabled ?? true;
         
     
        protected void OnClick()
	    {
		    if (this.IsActive)
		    {
				UpdateCommand.Execute();
				StateHasChanged();
		    }
	    }

	    protected void OnClick()
	    {
		    if (DataContext.CanIncrement)
		    {
				UpdateCommand.Execute();
				StateHasChanged();
		    }
	    }




public void OnIncrementChanged(int iCount)
{

}

public void CanIncrementChanged()
{
    UpdateCommand.CanExecute = this.IsActive;
}
*/
