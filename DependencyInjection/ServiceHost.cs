using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ShimmyMySherbet.DependencyInjection.Models;
using ShimmyMySherbet.DependencyInjection.Models.Exceptions;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;
using ShimmyMySherbet.DependencyInjection.Models.Lifetimes;

namespace ShimmyMySherbet.DependencyInjection
{
    public class ServiceHost : IServiceProvider, IHost
    {
        public ConfigurationBuilder Configuration { get; }

        public IContainerServiceCollection ServiceCollection { get; } = new ServicesContainer();
        public ITypeActivator Activator { get; } = new TypeActivator();
        public IContainerLifetime Lifetime { get; } = new ContainerLifetime();
        public IServiceProvider Services => this;

        public ServiceHost()
        {
            Configuration = new ConfigurationBuilder(ServiceCollection);
            RegisterSingleton(Lifetime);
            RegisterSingleton(ServiceCollection);
            RegisterSingleton(this);
        }

        #region "Registering"

        public void RegisterHostedService<T>()
        {
            RegisterSingleton<T>();
        }

        public void RegisterSingleton<T>()
        {
            ServiceCollection.AddService(new SingletonService(this, typeof(T)));
        }

        public void RegisterSingleton<T>(T instance) where T : notnull
        {
            ServiceCollection.AddService(new SingletonService(typeof(T), instance));
        }

        public void RegisterSingletonType(Type type)
        {
            ServiceCollection.AddService(new SingletonService(this, type));
        }

        public void RegisterSingletonType(Type type, object instance)
        {
            ServiceCollection.AddService(new SingletonService(type, instance));
        }

        public void RegisterTransient<T>()
        {
            ServiceCollection.AddService(new TransientService(this, typeof(T)));
        }

        public void RegisterTransient(Type t)
        {
            ServiceCollection.AddService(new TransientService(this, t));
        }

        #endregion "Registering"

        #region "Activating"

        public T ActivateType<T>(params object[] extras)
        {
            return Activator.ActivateType<T>(ServiceCollection, extras: extras);
        }

        public object ActivateType(Type type)
        {
            return Activator.ActivateType(ServiceCollection, type);
        }

        public object ActivateType(Type type, object[] extras)
        {
            return Activator.ActivateType(ServiceCollection, type, extras);
        }

        #endregion "Activating"

        public object GetService(Type serviceType)
        {
            if (serviceType.IsTypeLogger())
            {
                return ActivateType(serviceType);
            }

            var service = ServiceCollection.GetService(serviceType);
            if (service == null)
            {
                throw new ServiceNotFoundException(serviceType);
            }
            return service.GetInstance();
        }

        public object? TryGetService(Type serviceType)
        {
            if (serviceType.IsTypeLogger())
            {
                return ActivateType(serviceType);
            }

            var service = ServiceCollection.GetService(serviceType);
            if (service == null)
            {
                return null;
            }
            return service.GetInstance();
        }

        public T? TryGetService<T>()
        {
            if (typeof(T).IsTypeLogger())
            {
                return ActivateType<T>();
            }

            var service = ServiceCollection.GetService(typeof(T));
            if (service == null)
            {
                return default;
            }
            return (T)service.GetInstance();
        }

        public T[] GetServices<T>()
        {
            var services = ServiceCollection.GetServices(typeof(T));
            return services.Select(x => (T)x.GetInstance()).ToArray();
        }

        public void Run()
        {
            using (var startupServices = ServiceCollection.GetServiceIterator<IHostedService>())
            {
                while (startupServices.MoveNext())
                {
                    startupServices.Current.StartAsync(Lifetime.Token).Wait();
                }
            }

            Lifetime.WaitForShutdown();

            using var shutdownServices = ServiceCollection.GetServiceIterator<IHostedService>();
            while (shutdownServices.MoveNext())
            {
                shutdownServices.Current.StopAsync(default).Wait();
            }
        }

        public async Task RunAsync()
        {
            await StartAsync();

            await Lifetime.WaitForShutdownAsync();

            await StopAsync();
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            using var startupServices = ServiceCollection.GetServiceIterator<IHostedService>();
            while (startupServices.MoveNext())
            {
                await startupServices.Current.StartAsync(Lifetime.Token);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            using var shutdownServices = ServiceCollection.GetServiceIterator<IHostedService>();
            while (shutdownServices.MoveNext())
            {
                await shutdownServices.Current.StopAsync(default);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}