namespace Lyxbux.Mvvm
{
    public interface IViewModel<T> : IModel<T>
    {
        T Clone();
        void Copy(T model);
        bool Equals(T model);
        void Reset();
    }
}
