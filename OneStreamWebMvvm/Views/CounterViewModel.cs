using OneStreamWebUI.Mvvm.Toolkit;

namespace OneStreamWebMvvm
{
    public class CounterViewModel : ViewModelBase
    {
        private int currentCount = 1;
        private int maxCount = 3;      

        private bool isActive = true;
        public bool IsActive { get => isActive; set => SetProperty(ref isActive, value, nameof(IsActive)); }

        public int CurrentCount
        {
            get => currentCount;
            set => SetProperty(ref currentCount, value, nameof(CurrentCount));
        }

        public void IncrementCount()
        {
            if (this.currentCount < this.maxCount)
            {
                currentCount++;
            }
            else
            {
                this.isActive = !this.isActive;
            }
        }

        public void Reset()
        {
            this.currentCount = 1;
            this.isActive = !this.isActive;
        }
    }
}
