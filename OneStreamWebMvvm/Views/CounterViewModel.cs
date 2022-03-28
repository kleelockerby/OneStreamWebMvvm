using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class CounterViewModel : ViewModelBase
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

        public CounterViewModel()
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
