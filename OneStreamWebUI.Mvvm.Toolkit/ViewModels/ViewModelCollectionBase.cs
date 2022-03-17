using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class ViewModelCollectionBase<TViewModel> : ViewModelBase, IList<TViewModel>, INotifyCollectionChanged where TViewModel : class
    {
        private List<TViewModel> list = new List<TViewModel>();

        public List<TViewModel> List { get => list; set => list = value; }

        public int Count => list.Count;
        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public ViewModelCollectionBase() { }
        public ViewModelCollectionBase(IEnumerable<TViewModel> viewModel)
        {
            this.list = viewModel.ToList();
        }

        public void InitializeList(IEnumerable<TViewModel> viewModel)
        {
            this.list = viewModel.ToList();
        }

        public TViewModel this[int index]
        {
            get => list[index];
            set
            {
                var oldItem = list[index];
                list[index] = value;
                this.OnCountPropertyChanged();
                this.OnIndexerPropertyChanged();
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldItem, index));
            }
        }

        public void Add(TViewModel viewModel)
        {
            list.Add(viewModel);
            this.OnCountPropertyChanged();
            this.OnIndexerPropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, viewModel));
        }

        public bool Contains(TViewModel item)
        {
            return list.Contains(item);

        }

        public void CopyTo(TViewModel[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public void Clear()
        {
            list.Clear();
            this.OnCountPropertyChanged();
            this.OnIndexerPropertyChanged();
            OnCollectionReset();
        }

        public int IndexOf(TViewModel viewModel)
        {
            return list.IndexOf(viewModel);
        }

        public void Insert(int index, TViewModel item)
        {
            list.Insert(index, item);
            this.OnCountPropertyChanged();
            this.OnIndexerPropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        public bool Remove(TViewModel item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                bool result = list.Remove(item);
                if (result)
                {
                    OnCountPropertyChanged();
                    OnIndexerPropertyChanged();
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));

                }
                return result;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            var item = this[index];
            list.RemoveAt(index);
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        public void AddRange(IEnumerable<TViewModel> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public IEnumerator<TViewModel> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            CollectionChanged?.Invoke(this, args);
        }

        private void OnCollectionReset() => this.OnCollectionChanged(CollectionPropertyEvents.ResetCollectionChanged);

        private void OnCountPropertyChanged() => this.OnPropertyChanged(CollectionPropertyEvents.CountPropertyChanged);

        private void OnIndexerPropertyChanged() => this.OnPropertyChanged(CollectionPropertyEvents.IndexerPropertyChanged);
    }
}
