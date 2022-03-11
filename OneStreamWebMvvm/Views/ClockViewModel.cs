using System;
using System.Timers;
using OneStreamWebUI.Mvvm.Toolkit;
using Timer = System.Timers.Timer;

namespace OneStreamWebMvvm
{
    public class ClockViewModel : ViewModelBase, IDisposable
    {
        private readonly Timer timer;
        private DateTime dateTime = DateTime.Now;

        public ClockViewModel()
        {
            timer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            timer.Elapsed += TimerOnElapsed;
            timer.Start();
        }

        public DateTime DateTime
        {
            get => dateTime;
            set => SetProperty(ref dateTime, value, nameof(DateTime));
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            DateTime = DateTime.Now;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                timer.Dispose();
            }
        }

        ~ClockViewModel()
        {
            Dispose(false);
        }
    }
}
