using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShimmyMySherbet.DependencyInjection.Models.Interfaces;
using ShimmyMySherbet.DependencyInjection.Models.Lifetimes;

namespace ShimmyMySherbet.DependencyInjection.Models
{
    public static class Extensions
    {
        public static bool IsTypeLogger(this Type t)
        {
            return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ILogger<>);
        }

        public static IContainerService ToService(this ServiceDescriptor descriptor, ServiceHost provider)
        {
            var type = descriptor.ImplementationType ?? descriptor.ServiceType;
            switch (descriptor.Lifetime)
            {
                case ServiceLifetime.Scoped:
                case ServiceLifetime.Singleton:
                    if (descriptor.ImplementationFactory != null)
                    {
                        return new SingletonService(() => descriptor.ImplementationFactory(provider), type);
                    }
                    else if (descriptor.ImplementationInstance != null)
                    {
                        return new SingletonService(type, descriptor.ImplementationInstance);
                    }
                    else
                    {
                        return new SingletonService(provider, type);
                    }

                case ServiceLifetime.Transient:
                    if (descriptor.ImplementationFactory != null)
                    {
                        return new TransientService(() => descriptor.ImplementationFactory(provider), type);
                    }
                    else
                    {
                        return new TransientService(provider, type);
                    }

                default:
                    throw new NotSupportedException($"Unknown service lifetime '{descriptor.Lifetime}'");
            }
        }

        public static ServiceDescriptor ToDescriptor(this IContainerService service)
        {
            ServiceLifetime lifetime;
            if (service is SingletonService)
            {
                lifetime = ServiceLifetime.Singleton;
            }
            else if (service is TransientService)
            {
                lifetime = ServiceLifetime.Transient;
            }
            else if (service is ScopeService)
            {
                lifetime = ServiceLifetime.Scoped;
            }
            else
            {
                lifetime = ServiceLifetime.Singleton;
            }
            return new ServiceDescriptor(service.Type, (_) => service.GetInstance(), lifetime);
        }

        public static void RegisterSingleton<O>(this ServiceHost host, Func<O> factory) =>
            host.RegisterSingleton(factory, typeof(O));

        public static void RegisterSingleton2<O>(this ServiceHost host, Func<O, object> factory) =>
            host.RegisterSingleton(factory, typeof(O));

        public static void RegisterSingleton<O, A1>(this ServiceHost host, Func<A1, O> factory) =>
            host.RegisterSingleton(factory, typeof(O));

        public static void RegisterSingleton<O, A1, A2>(this ServiceHost host, Func<A1, A2, O> factory) =>
            host.RegisterSingleton(factory, typeof(O));

        public static void RegisterSingleton<O, A1, A2, A3>(this ServiceHost host, Func<A1, A2, A3, O> factory) =>
            host.RegisterSingleton(factory, typeof(O));

        public static void RegisterSingleton<O, A1, A2, A3, A4>(this ServiceHost host, Func<A1, A2, A3, A4, O> factory) =>
            host.RegisterSingleton(factory, typeof(O));

        public static void RegisterSingleton<O, A1, A2, A3, A4, A5>(this ServiceHost host, Func<A1, A2, A3, A4, A5, O> factory) =>
            host.RegisterSingleton(factory, typeof(O));

        public static void RegisterSingleton<O, A1, A2, A3, A4, A5, A6>(this ServiceHost host, Func<A1, A2, A3, A4, A5, A6, O> factory) =>
            host.RegisterSingleton(factory, typeof(O));

        public static void RegisterTransient<O>(this ServiceHost host, Func<O> factory) =>
            host.RegisterTransient(factory, typeof(O));

        public static void RegisterTransient<O, A1>(this ServiceHost host, Func<A1, O> factory) =>
            host.RegisterTransient(factory, typeof(O));

        public static void RegisterTransient<O, A1, A2>(this ServiceHost host, Func<A1, A2, O> factory) =>
            host.RegisterTransient(factory, typeof(O));

        public static void RegisterTransient<O, A1, A2, A3>(this ServiceHost host, Func<A1, A2, A3, O> factory) =>
            host.RegisterTransient(factory, typeof(O));

        public static void RegisterTransient<O, A1, A2, A3, A4>(this ServiceHost host, Func<A1, A2, A3, A4, O> factory) =>
            host.RegisterTransient(factory, typeof(O));

        public static void RegisterTransient<O, A1, A2, A3, A4, A5>(this ServiceHost host, Func<A1, A2, A3, A4, A5, O> factory) =>
            host.RegisterTransient(factory, typeof(O));

        public static void RegisterTransient<O, A1, A2, A3, A4, A5, A6>(this ServiceHost host, Func<A1, A2, A3, A4, A5, A6, O> factory) =>
            host.RegisterTransient(factory, typeof(O));
    }
}