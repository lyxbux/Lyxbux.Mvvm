namespace Lyxbux.Mvvm
{
    public sealed class DMList<T> : List<T>, IDataModel<DMList<T>>
    {
        public DMList() : base() { }
        public DMList(IEnumerable<T> collection) : base(collection) { }
        public DMList(int capacity) : base(capacity) { }
    }
}
