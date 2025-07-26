namespace Lyxbux.Mvvm
{
    public interface ISafeService : IService
    {
        T Invoke<T>(Func<T> callback);
        void Invoke(Action callback);
    }
}
