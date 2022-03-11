using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class CounterViewModel : ViewModelBase
    {
        private int currentCount;

        public int CurrentCount
        {
            get => currentCount;
            set => SetProperty(ref currentCount, value, nameof(CurrentCount));
        }

        public void IncrementCount()
        {
            CurrentCount++;
        }
    }
}
