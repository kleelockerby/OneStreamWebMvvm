using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace OneStreamWebUI.Mvvm.Toolkit
{
    public class ViewModelCollection<TItem> : IList<TItem>, INotifyCollectionChanged where TItem : class
    {
        private List<TItem> list = new List<TItem>();
        public List<TItem> List { get => list; set => list = value; }

        public int Count => list.Count;
        public bool IsReadOnly => false;

        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public ViewModelCollection() { }
        public ViewModelCollection(IEnumerable<TItem> item)
        {
            this.list = item.ToList();
        }

        public void Initialize(IEnumerable<TItem> item)
        {
            this.list = item.ToList();
        }

        public TItem this[int index]
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

        public void Add(TItem item)
        {
            list.Add(item);
            this.OnCountPropertyChanged();
            this.OnIndexerPropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public bool Contains(TItem item)
        {
            return list.Contains(item);

        }

        public void CopyTo(TItem[] array, int arrayIndex)
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

        public int IndexOf(TItem item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, TItem item)
        {
            list.Insert(index, item);
            this.OnCountPropertyChanged();
            this.OnIndexerPropertyChanged();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        public bool Remove(TItem item)
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

        public void AddRange(IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        private static List<TItem> CreateCopy(IEnumerable<TItem> collection, string paramName) => collection != null ? new List<TItem>(collection) : throw new ArgumentNullException(paramName);

        public IEnumerator<TItem> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler? propertyChanged = this.PropertyChanged;
            if (propertyChanged == null)
            {
                return;
            }
            propertyChanged((object)this, e);
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
