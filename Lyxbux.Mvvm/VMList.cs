using System.Collections.ObjectModel;

namespace Lyxbux.Mvvm
{
    public sealed class VMList<T> : ObservableCollection<T>, IViewModel<VMList<T>>
    {
        public VMList() : base() { }
        public VMList(IEnumerable<T> collection) : base(collection) { }
        public VMList(List<T> list) : base(list) { }

        public VMList<T> Clone()
        {
            VMList<T> list = new VMList<T>();

            foreach (T item in this)
            {
                if (item is IViewModel<T> viewModel)
                {
                    list.Add(viewModel.Clone());
                }
                else
                {
                    list.Add(item);
                }
            }

            return list;
        }
        public void Copy(VMList<T> model)
        {
            Clear();

            foreach (T item in model)
            {
                if (item is IViewModel<T> viewModel)
                {
                    Add(viewModel.Clone());
                }
                else
                {
                    Add(item);
                }
            }
        }
        public bool Equals(VMList<T> model)
        {
            if (model == null)
            {
                return false;
            }
            if (Count != model.Count)
            {
                return false;
            }
            for (int i = 0; i < Count; i++)
            {
                T item1 = this[i];
                T item2 = model[i];

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
        }
        public void Reset()
        {
            foreach (T item in this)
            {
                if (item is IViewModel<T> viewModel)
                {
                    viewModel.Reset();
                }
                else
                {
                    // N/A
                }
            }

            Clear();
        }
    }
}
