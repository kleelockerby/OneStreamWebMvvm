using System;
using System.ComponentModel;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public abstract class ModelBase
    {
        protected bool IsBatchUpdate;

        public event PropertyChangedEventHandler? PropertyChanged;
        public Action<object, bool>? ModelChanged;

        public void BeginUpdate()
        {
            this.IsBatchUpdate = true;
        }

        public void EndUpdate()
        {
            this.IsBatchUpdate = false;
        }

        public abstract void UpdateModel(object model);

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
