namespace Lyxbux.Mvvm
{
    public sealed class PseudoSafeService : ISafeService
    {
        public T Invoke<T>(Func<T> callback)
        {
            return callback();
        }
        public void Invoke(Action callback)
        {
            callback();
        }
    }
}
