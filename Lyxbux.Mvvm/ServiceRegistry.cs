using Lyxbux.Registry;

namespace Lyxbux.Mvvm
{
    public static class ServiceRegistry
    {
        public static bool RegisterService(string serviceName, IService service)
        {
            return AppRegistry.Register($@"Services/{serviceName}", service);
        }
        public static bool UnregisterService(string serviceName)
        {
            return AppRegistry.Unregister($@"Services/{serviceName}");
        }
        public static IService? GetService(string serviceName)
        {
            if (AppRegistry.Get($@"Services/{serviceName}", out IService? service))
            {
                return service;
            }

            return null;
        }
    }
}
