using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Lyxbux.Mvvm
{
    public sealed class VMList<T> : IEnumerable, IEnumerable<T>, ICollection, ICollection<T>, IReadOnlyCollection<T>, IList, IList<T>, IReadOnlyList<T>, INotifyPropertyChanged, INotifyCollectionChanged, IViewModel<VMList<T>>
    {
        private static readonly PseudoSafeService pseudoSafeService = new PseudoSafeService();
        private static ISafeService SafeService => ServiceRegistry.GetSafeService() ?? pseudoSafeService;
        private readonly object syncRoot = new object();
        private readonly List<T> list;

        #region Direct Public Properties
        public T this[int index]
        {
            set
            {
                SafeService.Invoke(() => ReplaceItem(index, value));
            }
            get
            {
                return SafeService.Invoke(() => list[index]);
            }
        }
        public int Count
        {
            get
            {
                return SafeService.Invoke(() => list.Count);
            }
        }
        public bool IsSynchronized
        {
            get
            {
                return SafeService.Invoke(() => false);
            }
        }
        public object SyncRoot
        {
            get
            {
                return SafeService.Invoke(() => syncRoot);
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return SafeService.Invoke(() => false);
            }
        }
        public bool IsFixedSize
        {
            get
            {
                return SafeService.Invoke(() => false);
            }
        }
        #endregion Direct Public Properties

        #region Indirect Public Properties
        object? IList.this[int index]
        {
            set
            {
                this[index] = (T)value!;
            }
            get
            {
                return this[index];
            }
        }
        T IList<T>.this[int index]
        {
            set
            {
                this[index] = value;
            }
            get
            {
                return this[index];
            }
        }
        T IReadOnlyList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
        }
        int ICollection.Count
        {
            get
            {
                return Count;
            }
        }
        int ICollection<T>.Count
        {
            get
            {
                return Count;
            }
        }
        int IReadOnlyCollection<T>.Count
        {
            get
            {
                return Count;
            }
        }
        bool ICollection.IsSynchronized
        {
            get
            {
                return IsSynchronized;
            }
        }
        object ICollection.SyncRoot
        {
            get
            {
                return SyncRoot;
            }
        }
        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return IsReadOnly;
            }
        }
        bool IList.IsReadOnly
        {
            get
            {
                return IsReadOnly;
            }
        }
        bool IList.IsFixedSize
        {
            get
            {
                return IsFixedSize;
            }
        }
        #endregion Indirect Public Properties

        #region Constructors
        public VMList()
        {
            this.list = SafeService.Invoke(() => new List<T>());
        }
        public VMList(IEnumerable<T> collection)
        {
            this.list = SafeService.Invoke(() => new List<T>(collection));
        }
        public VMList(List<T> list)
        {
            this.list = SafeService.Invoke(() => new List<T>(list));
        }
        #endregion Constructors

        #region Advance Public Methods
        public VMList<T> Clone()
        {
            return SafeService.Invoke(() =>
            {
                VMList<T> model = new VMList<T>();

                foreach (T item in list)
                {
                    int index = model.list.Count;
                    if (item is IViewModel<T> viewModel)
                    {
                        model.AddItem(index, viewModel.Clone());
                    }
                    else
                    {
                        model.AddItem(index, item);
                    }
                }

                return model;
            });
        }
        public void Copy(VMList<T> model)
        {
            SafeService.Invoke(() =>
            {
                ResetItems();

                foreach (T item in model)
                {
                    int index = list.Count;
                    if (item is IViewModel<T> viewModel)
                    {
                        AddItem(index, viewModel.Clone());
                    }
                    else
                    {
                        AddItem(index, item);
                    }
                }
            });
        }
        public bool Equals(VMList<T> model)
        {
            return SafeService.Invoke(() =>
            {
                if (model == null)
                {
                    return false;
                }
                if (list.Count != model.list.Count)
                {
                    return false;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    T item1 = list[i];
                    T item2 = model.list[i];

                    if (item1 == null)
                    {
                        if (item2 != null)
                        {
                            return false;
                        }

                        continue;
                    }

                    if (item1 is IViewModel<T> viewModel)
                    {
                        if (!viewModel.Equals(item2))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!item1.Equals(item2))
                        {
                            return false;
                        }
                    }
                }

                return true;
            });
        }
        public void Reset()
        {
            SafeService.Invoke(() =>
            {
                while (list.Count > 0)
                {
                    int index = list.Count - 1;
                    T item = list[index];
                    RemoveItem(index);

                    if (item is IViewModel<T> viewModel)
                    {
                        viewModel.Reset();
                    }
                    else
                    {
                        // N/A
                    }
                }
            });
        }
        #endregion Advance Public Methods

        #region Direct Public Methods
        public void Add(T item)
        {
            SafeService.Invoke(() =>
            {
                int index = list.Count;
                AddItem(index, item);
            });
        }
        public bool Remove(T item)
        {
            return SafeService.Invoke(() =>
            {
                int index = list.IndexOf(item);
                if (index < 0)
                {
                    return false;
                }

                RemoveItem(index);
                return true;
            });
        }
        public void Clear()
        {
            SafeService.Invoke(() => ResetItems());
        }
        public void Insert(int index, T item)
        {
            SafeService.Invoke(() => AddItem(index, item));
        }
        public void RemoveAt(int index)
        {
            SafeService.Invoke(() => RemoveItem(index));
        }
        public void Move(int oldIndex, int newIndex)
        {
            SafeService.Invoke(() => MoveItem(oldIndex, newIndex));
        }
        public bool Contains(T item)
        {
            return SafeService.Invoke(() => list.Contains(item));
        }
        public int IndexOf(T item)
        {
            return SafeService.Invoke(() => list.IndexOf(item));
        }
        public void CopyTo(T[] array, int index)
        {
            SafeService.Invoke(() => list.CopyTo(array, index));
        }
        public IEnumerator<T> GetEnumerator()
        {
            return SafeService.Invoke(() => list.GetEnumerator());
        }
        #endregion Direct Public Methods

        #region Indirect Public Methods
        void ICollection<T>.Add(T item)
        {
            Add(item);
        }
        int IList.Add(object? value)
        {
            Add((T)value!);
            return Count - 1;
        }
        bool ICollection<T>.Remove(T item)
        {
            return Remove(item);
        }
        void IList.Remove(object? value)
        {
            Remove((T)value!);
        }
        void ICollection<T>.Clear()
        {
            Clear();
        }
        void IList.Clear()
        {
            Clear();
        }
        void IList.Insert(int index, object? value)
        {
            Insert(index, (T)value!);
        }
        void IList<T>.Insert(int index, T item)
        {
            Insert(index, item);
        }
        void IList.RemoveAt(int index)
        {
            RemoveAt(index);
        }
        void IList<T>.RemoveAt(int index)
        {
            RemoveAt(index);
        }
        bool ICollection<T>.Contains(T item)
        {
            return Contains(item);
        }
        bool IList.Contains(object? value)
        {
            return Contains((T)value!);
        }
        int IList.IndexOf(object? value)
        {
            return IndexOf((T)value!);
        }
        int IList<T>.IndexOf(T item)
        {
            return IndexOf(item);
        }
        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo((T[])array, index);
        }
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion Indirect Public Methods

        #region Private Methods
        private void AddItem(int index, T item)
        {
            list.Insert(index, item);
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnAddCollectionChanged(item, index);
        }
        private void RemoveItem(int index)
        {
            T removeItem = list[index];
            list.RemoveAt(index);
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnRemoveCollectionChanged(removeItem, index);
        }
        private void ReplaceItem(int index, T item)
        {
            T oldItem = list[index];
            list[index] = item;
            OnIndexerPropertyChanged();
            OnReplaceCollectionChanged(item, oldItem, index);
        }
        private void MoveItem(int oldIndex, int newIndex)
        {
            T moveItem = list[oldIndex];
            list.RemoveAt(oldIndex);
            list.Insert(newIndex, moveItem);
            OnIndexerPropertyChanged();
            OnMoveCollectionChanged(moveItem, newIndex, oldIndex);
        }
        private void ResetItems()
        {
            list.Clear();
            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnResetCollectionChanged();
        }
        #endregion Private Methods

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private void OnIndexerPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
        }
        private void OnCountPropertyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Count"));
        }
        private void OnAddCollectionChanged(object? changedItem, int index)
        {
            NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Add;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem, index));
        }
        private void OnRemoveCollectionChanged(object? changedItem, int index)
        {
            NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Remove;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem, index));
        }
        private void OnReplaceCollectionChanged(object? newItem, object? oldItem, int index)
        {
            NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Replace;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }
        private void OnMoveCollectionChanged(object? changedItem, int index, int oldIndex)
        {
            NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Move;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem, index, oldIndex));
        }
        private void OnResetCollectionChanged()
        {
            NotifyCollectionChangedAction action = NotifyCollectionChangedAction.Reset;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action));
        }
        #endregion Events
    }
}
